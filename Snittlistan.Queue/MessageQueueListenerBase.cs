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
    private readonly MessageQueue[] importMessageQueues;
    private readonly MessageQueue errorMessageQueue;
    private readonly Counter counter = new();
    private readonly MessageQueueProcessorSettings settings;
    private volatile bool isClosing;

    protected MessageQueueListenerBase(MessageQueueProcessorSettings settings)
    {
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
            "using {numberOfThreads} worker threads for {readQueue}",
            numberOfThreads,
            settings.ReadQueue);
        importMessageQueues = Enumerable.Range(0, numberOfThreads).Select(
            x =>
            {
                MessageQueue queue = new(settings.ReadQueue, QueueAccessMode.SendAndReceive);
                queue.MessageReadPropertyFilter.SetAll();
                return queue;
            }).ToArray();

        // create error queue automatically, if it doesn't exist
        if (settings.AutoCreateQueues && settings.DropFailedMessages == false)
        {
            CreateQueue(settings.ErrorQueue);
        }

        errorMessageQueue = new MessageQueue(settings.ErrorQueue, QueueAccessMode.Send);
        this.settings = settings;
    }

    public void Start()
    {
        Logger.Info("starting msmq listener");
        foreach (MessageQueue importMessageQueue in importMessageQueues)
        {
            importMessageQueue.PeekCompleted += Queue_PeekCompleted;
            _ = importMessageQueue.BeginPeek();
        }
    }

    public void Stop()
    {
        isClosing = true;
        Logger.Info("stopping msmq listener");
        foreach (MessageQueue importMessageQueue in importMessageQueues)
        {
            importMessageQueue.PeekCompleted -= Queue_PeekCompleted;
            importMessageQueue.Close();
        }

        errorMessageQueue.Close();

        // wait (at most 10 seconds) for processor threads to finish working
        Stopwatch sw = Stopwatch.StartNew();
        while (counter.Value > 0 && sw.ElapsedMilliseconds < 10000)
        {
            Thread.Sleep(100);
        }

        int value = counter.Value;
        if (value > 0)
        {
            Logger.Error(
                "failed to stop workers for {importQueue} ({count} remaining)",
                settings.ReadQueue,
                value);
        }

        isClosing = false;
    }

    public void Dispose()
    {
        foreach (MessageQueue importMessageQueue in importMessageQueues)
        {
            importMessageQueue.Dispose();
        }

        errorMessageQueue.Dispose();
    }

    protected abstract Task DoHandle(string contents);

    private static void CreateQueue(string queueName)
    {
        if (MessageQueue.Exists(queueName))
        {
            return;
        }

        Logger.Info("creating queue {queueName}", queueName);
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
            if (isClosing == false)
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
            counter.Increment();
            IDisposable disposable = NestedDiagnosticsLogicalContext.Push(message.Id);
            try
            {
                Logger.Info("start");
                await TryProcessMessage(message, sourceQueue, messageQueueTransaction);
            }
            finally
            {
                Logger.Info("end");
                disposable.Dispose();
            }
        }
        finally
        {
            counter.Decrement();
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
        catch (Exception ex) when (settings.DropFailedMessages)
        {
            Logger.Warn(ex);
            Logger.Warn("dropping message {id}", message.Id);
        }
        catch (Exception ex)
        {
            Logger.Warn(ex);
            message.AppSpecific++;
            Logger.Warn("message retry #{count}", message.AppSpecific);
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
            "message {id} reached error count {count}, moving to error queue.",
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
        errorMessageQueue.Send(message, messageQueueTransaction);
    }
}
