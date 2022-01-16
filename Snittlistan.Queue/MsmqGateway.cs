using System.Messaging;
using NLog;
using Snittlistan.Queue.Messages;

#nullable enable

namespace Snittlistan.Queue;
public static class MsmqGateway
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private static MessageQueue? messageQueue;

    public static void Initialize(string path)
    {
        messageQueue = new MessageQueue(path)
        {
            Formatter = new JsonMessageFormatter()
        };
    }

    public static MsmqTransactionScope Create()
    {
        return messageQueue == null
            ? throw new Exception("Initialize MsmqGateway")
            : new MsmqTransactionScope(messageQueue);
    }

    public class MsmqTransactionScope : IMsmqTransaction, IDisposable
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
