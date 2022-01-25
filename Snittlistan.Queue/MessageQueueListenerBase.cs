#nullable enable

using System.Diagnostics;
using System.IO;
using System.Messaging;
using System.Text;
using NLog;

namespace Snittlistan.Queue;

public abstract class MessageQueueListenerBase : IDisposable
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly string _importQueue;
    private readonly MessageQueue[] _importMessageQueues;
    private readonly MessageQueue _errorMessageQueue;
    private readonly Counter _counter = new();
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
        int numberOfThreads = Math.Max(1, settings.WorkerThreadCount);
        int maxThreads = Environment.ProcessorCount * 2;
        numberOfThreads = Math.Min(numberOfThreads, maxThreads);

        Logger.Info(
            "Using {numberOfThreads} worker threads for {readQueue}",
            numberOfThreads,
            settings.ReadQueue);
        _importMessageQueues = Enumerable.Range(0, numberOfThreads).Select(
            x =>
            {
                MessageQueue queue = new(settings.ReadQueue, QueueAccessMode.SendAndReceive);
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
        Logger.Info("Starting msmq listener");
        foreach (MessageQueue importMessageQueue in _importMessageQueues)
        {
            importMessageQueue.PeekCompleted += Queue_PeekCompleted;
            _ = importMessageQueue.BeginPeek();
        }
    }

    public void Stop()
    {
        _isClosing = true;
        Logger.Info("Stopping msmq listener");
        foreach (MessageQueue importMessageQueue in _importMessageQueues)
        {
            importMessageQueue.PeekCompleted -= Queue_PeekCompleted;
            importMessageQueue.Close();
        }

        _errorMessageQueue.Close();

        // wait (at most 10 seconds) for processor threads to finish working
        Stopwatch sw = Stopwatch.StartNew();
        while (_counter.Value > 0 && sw.ElapsedMilliseconds < 10000)
        {
            Thread.Sleep(100);
        }

        int value = _counter.Value;
        if (value > 0)
        {
            Logger.Error(
                "Failed to stop workers for {importQueue} ({count} remaining)",
                _importQueue,
                value);
        }

        _isClosing = false;
    }

    public void Dispose()
    {
        foreach (MessageQueue importMessageQueue in _importMessageQueues)
        {
            importMessageQueue.Dispose();
        }

        _errorMessageQueue.Dispose();
    }

    protected abstract Task DoHandle(string contents);

    private static void CreateQueue(string queueName)
    {
        if (MessageQueue.Exists(queueName))
        {
            return;
        }

        Logger.Info("Creating queue {queueName}", queueName);
        MessageQueue queue = MessageQueue.Create(queueName, true);
        queue.UseJournalQueue = true;
        queue.MaximumJournalSize = 10 * 1024;
        queue.SetPermissions("Everyone", MessageQueueAccessRights.FullControl, AccessControlEntryType.Allow);
    }

    private async void Queue_PeekCompleted(object sender, PeekCompletedEventArgs e)
    {
        // end peek operation
        MessageQueue queue = (MessageQueue)sender;

        _ = queue.EndPeek(e.AsyncResult);

        // read message transactionally
        MessageQueueTransaction? transaction = null;
        try
        {
            transaction = new MessageQueueTransaction();
            transaction.Begin();

            // if the queue closes after the transaction begins,
            // but before the call to Receive, then an exception
            // will be thrown and the transaction will be aborted
            // leaving the message to be processed next time
            Message message = queue.Receive(TimeSpan.Zero, transaction);
            await ProcessMessage(message, queue, transaction);
            transaction.Commit();
        }
        catch (MessageQueueException ex)
        {
            transaction?.Abort();

            // timeout is expected here, sometimes
            if (ex.MessageQueueErrorCode != MessageQueueErrorCode.IOTimeout)
            {
                Logger.Error(ex);
            }
        }
        catch (Exception ex)
        {
            // something unexpected happened
            transaction?.Abort();

            Logger.Error(ex);
        }
        finally
        {
            transaction?.Dispose();

            // start new peek operation
            if (_isClosing == false)
            {
                _ = queue.BeginPeek();
            }
        }
    }

    private async Task ProcessMessage(
        Message message,
        MessageQueue sourceQueue,
        MessageQueueTransaction messageQueueTransaction)
    {
        try
        {
            _counter.Increment();
            IDisposable disposable = NestedDiagnosticsLogicalContext.Push(message.Id);
            try
            {
                Logger.Info("Start");
                await TryProcessMessage(message, sourceQueue, messageQueueTransaction);
            }
            finally
            {
                Logger.Info("End");
                disposable.Dispose();
            }
        }
        finally
        {
            _counter.Decrement();
        }
    }

    private async Task TryProcessMessage(Message message, MessageQueue sourceQueue, MessageQueueTransaction messageQueueTransaction)
    {
        // get contents of message (don't dispose the reader
        // as that will dispose the stream and the message
        // can't be moved to the error queue)
        string contents = new StreamReader(message.BodyStream).ReadToEnd();

        const int MaximumTries = 1;
        try
        {
            await DoHandle(contents);
        }
        catch (Exception ex)
        {
            Logger.Warn(ex);
            message.AppSpecific++;
            Logger.Warn("Message retry #{count}", message.AppSpecific);
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
        Logger.Error(
            "Message {id} reached error count {count}, moving to error queue.",
            message.Id,
            errorCount);

        // reset error count
        message.AppSpecific = 0;

        // place exception details in the extension property
        message.Extension = Encoding.UTF8.GetBytes(ex.ToString());

        // maximum length allowed is 249
        string exceptionMessage = ex.Message;
        message.Label = exceptionMessage.Substring(0, Math.Min(exceptionMessage.Length, 249));

        // send message to error queue
        _errorMessageQueue.Send(message, messageQueueTransaction);
    }
}
