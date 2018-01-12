using System;
using System.Messaging;
using System.Reflection;
using log4net;
using Snittlistan.Queue.Messages;

namespace Snittlistan.Queue
{
    public static class MsmqGateway
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static MessageQueue messageQueue;
        private static MessageQueueTransaction transaction;

        public static void Initialize(string path)
        {
            messageQueue = new MessageQueue(path)
            {
                Formatter = new JsonMessageFormatter()
            };
        }

        public static void BeginTransaction()
        {
            if (messageQueue == null) throw new Exception("Initialize MsmqGateway");
            transaction = new MessageQueueTransaction();
            transaction.Begin();
        }

        public static void PublishMessage(MessageEnvelope envelope)
        {
            if (messageQueue == null) throw new Exception("Initialize MsmqGateway");
            if (transaction == null) throw new Exception("BeginTransaction first");

            Log.InfoFormat("Sending {0}", envelope);
            messageQueue.Send(envelope, transaction);
        }

        public static void CommitTransaction()
        {
            transaction.Commit();
            transaction.Dispose();
            transaction = null;
        }
    }
}