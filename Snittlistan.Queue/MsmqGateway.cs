#nullable enable

using System.Messaging;
using NLog;
using Snittlistan.Queue.Messages;

namespace Snittlistan.Queue;

public class MsmqGateway : IDisposable
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly MessageQueue messageQueue;

    public MsmqGateway(string path)
    {
        messageQueue = new MessageQueue(path)
        {
            Formatter = new JsonMessageFormatter()
        };
    }

    public MsmqTransactionScope Create()
    {
        return new MsmqTransactionScope(messageQueue);
    }

    public void Dispose()
    {
        messageQueue.Dispose();
    }

    public class MsmqTransactionScope : IDisposable
    {
        private readonly MessageQueueTransaction transaction = new();
        private readonly MessageQueue messageQueue;

        public MsmqTransactionScope(MessageQueue messageQueue)
        {
            transaction.Begin();
            this.messageQueue = messageQueue;
        }

        public void Send(MessageEnvelope envelope)
        {
            Logger.Info("Sending {@envelope}", envelope);
            messageQueue.Send(envelope, transaction);
        }

        public void Commit()
        {
            try
            {
                transaction.Commit();
            }
            catch
            {
                transaction.Abort();
                throw;
            }
        }

        public void Dispose()
        {
            transaction.Dispose();
        }
    }
}
