#nullable enable

namespace Snittlistan.Queue.Messages
{
    using System;

    public class MessageContext<TTask> where TTask : ITask
    {
        private readonly int tenantId;
        private readonly IMsmqTransaction msmqTransaction;

        public MessageContext(
            TTask task,
            int tenantId,
            Guid correlationId,
            Guid causationId,
            IMsmqTransaction msmqTransaction)
        {
            Task = task;
            this.tenantId = tenantId;
            CorrelationId = correlationId;
            CausationId = causationId;
            this.msmqTransaction = msmqTransaction;
        }

        public TTask Task { get; }

        public Guid CorrelationId { get; }

        public Guid CausationId { get; }

        public void PublishMessage(ITask task)
        {
            MessageEnvelope envelope = new(
                task,
                tenantId,
                CorrelationId,
                CausationId,
                Guid.NewGuid());
            msmqTransaction.PublishMessage(envelope);
        }
    }
}
