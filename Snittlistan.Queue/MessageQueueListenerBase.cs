using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Reflection;
using System.Text;
using System.Threading;
using log4net;

namespace Snittlistan.Queue
{
    public abstract class MessageQueueListenerBase : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string _importQueue;
        private readonly MessageQueue[] _importMessageQueues;
        private readonly MessageQueue _errorMessageQueue;
        private readonly Counter _counter = new Counter();
        private volatile bool _isClosing;

        protected MessageQueueListenerBase(MessageQueueProcessorSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            _importQueue = settings.ReadQueue;

            // create import queue automatically, if it doesn't exist
            if (settings.AutoCreateQueues)
            {
                CreateQueue(settings.ReadQueue);
            }

            // limit threads to be between 1 and (# processors * 2)
            var numberOfThreads = Math.Max(1, settings.WorkerThreadCount);
            var maxThreads = Environment.ProcessorCount * 2;
            numberOfThreads = Math.Min(numberOfThreads, maxThreads);

            Log.InfoFormat(
                "Using {0} worker threads for {1}",
                numberOfThreads,
                settings.ReadQueue);
            _importMessageQueues = Enumerable.Range(0, numberOfThreads).Select(
                x =>
                {
                    var queue = new MessageQueue(settings.ReadQueue, QueueAccessMode.SendAndReceive);
                    queue.MessageReadPropertyFilter.SetAll();
                    return queue;
                }).ToArray();

            // create error queue automatically, if it doesn't exist
            if (settings.AutoCreateQueues)
            {
                CreateQueue(settings.ErrorQueue);
            }

            _errorMessageQueue = new MessageQueue(settings.ErrorQueue, QueueAccessMode.Send);
        }

        public void Start()
        {
            Log.Info("Starting msmq listener");
            foreach (var importMessageQueue in _importMessageQueues)
            {
                importMessageQueue.PeekCompleted += Queue_PeekCompleted;
                importMessageQueue.BeginPeek();
            }
        }

        public void Stop()
        {
            _isClosing = true;
            Log.Info("Stopping msmq listener");
            foreach (var importMessageQueue in _importMessageQueues)
            {
                importMessageQueue.PeekCompleted -= Queue_PeekCompleted;
                importMessageQueue.Close();
            }

            _errorMessageQueue.Close();

            // wait (at most 10 seconds) for processor threads to finish working
            var sw = Stopwatch.StartNew();
            while (_counter.Value > 0 && sw.ElapsedMilliseconds < 10000)
            {
                Thread.Sleep(100);
            }

            var value = _counter.Value;
            if (value > 0)
            {
                Log.ErrorFormat(
                    "Failed to stop workers for {0} ({1} remaining)",
                    _importQueue,
                    value);
            }

            _isClosing = false;
        }

        public void Dispose()
        {
            foreach (var importMessageQueue in _importMessageQueues)
            {
                importMessageQueue.Dispose();
            }

            _errorMessageQueue.Dispose();
        }

        protected abstract void DoHandle(string contents);

        private static void CreateQueue(string queueName)
        {
            if (MessageQueue.Exists(queueName))
            {
                return;
            }

            Log.InfoFormat("Creating queue {0}", queueName);
            var queue = MessageQueue.Create(queueName, true);
            queue.UseJournalQueue = true;
            queue.MaximumJournalSize = 10 * 1024;
            queue.SetPermissions("Everyone", MessageQueueAccessRights.FullControl, AccessControlEntryType.Allow);
        }

        private void Queue_PeekCompleted(object sender, PeekCompletedEventArgs e)
        {
            // end peek operation
            var queue = (MessageQueue)sender;

            queue.EndPeek(e.AsyncResult);

            // read message transactionally
            MessageQueueTransaction transaction = null;
            try
            {
                transaction = new MessageQueueTransaction();
                transaction.Begin();

                // if the queue closes after the transaction begins,
                // but before the call to Receive, then an exception
                // will be thrown and the transaction will be aborted
                // leaving the message to be processed next time
                var message = queue.Receive(TimeSpan.Zero, transaction);
                ProcessMessage(message, queue, transaction);
                transaction.Commit();
            }
            catch (MessageQueueException ex)
            {
                transaction?.Abort();

                // timeout is expected here, sometimes
                if (ex.MessageQueueErrorCode != MessageQueueErrorCode.IOTimeout)
                {
                    Log.Error(ex.GetType().ToString(), ex);
                }
            }
            catch (Exception ex)
            {
                // something unexpected happened
                transaction?.Abort();

                Log.Error(ex.GetType().ToString(), ex);
            }
            finally
            {
                transaction?.Dispose();

                // start new peek operation
                if (_isClosing == false)
                {
                    queue.BeginPeek();
                }
            }
        }

        private void ProcessMessage(Message message, MessageQueue sourceQueue, MessageQueueTransaction messageQueueTransaction)
        {
            try
            {
                _counter.Increment();
                var disposable = ThreadContext.Stacks["NDC"].Push(message.Id);
                try
                {
                    Log.InfoFormat("Start");
                    TryProcessMessage(message, sourceQueue, messageQueueTransaction);
                }
                finally
                {
                    Log.InfoFormat("End");
                    disposable.Dispose();
                }
            }
            finally
            {
                _counter.Decrement();
            }
        }

        private void TryProcessMessage(Message message, MessageQueue sourceQueue, MessageQueueTransaction messageQueueTransaction)
        {
            // get contents of message (don't dispose the reader
            // as that will dispose the stream and the message
            // can't be moved to the error queue)
            var contents = new StreamReader(message.BodyStream).ReadToEnd();

            // try to process message up to 5 times
            const int MaximumTries = 5;
            try
            {
                DoHandle(contents);
            }
            catch (Exception ex)
            {
                Log.Warn(ex.GetType().ToString(), ex);
                message.AppSpecific++;
                Log.WarnFormat("Message retry #{0}", message.AppSpecific);
                if (message.AppSpecific >= MaximumTries)
                {
                    MoveToErrorQueue(message, ex, messageQueueTransaction, MaximumTries);
                }
                else
                {
                    // send it back to import queue
                    sourceQueue.Send(message, messageQueueTransaction);
                }
            }
        }

        private void MoveToErrorQueue(Message message, Exception ex, MessageQueueTransaction messageQueueTransaction, int errorCount)
        {
            Log.ErrorFormat(
                "Message {0} reached error count {1}, moving to error queue.",
                message.Id,
                errorCount);

            // reset error count
            message.AppSpecific = 0;

            // place exception details in the extension property
            message.Extension = Encoding.UTF8.GetBytes(ex.ToString());

            // maximum length allowed is 249
            var exceptionMessage = ex.Message;
            message.Label = exceptionMessage.Substring(0, Math.Min(exceptionMessage.Length, 249));

            // send message to error queue
            _errorMessageQueue.Send(message, messageQueueTransaction);
        }
    }
}