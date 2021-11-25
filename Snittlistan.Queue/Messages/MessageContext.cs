#nullable enable

namespace Snittlistan.Queue.Messages
{
    using System;
    using System.Threading.Tasks;

    public delegate Task PublishMessageDelegate(
        ITask task,
        int tenantId,
        Guid causationId,
        IMsmqTransaction msmqTransaction);

    public interface IMessageContext
    {
        PublishMessageDelegate PublishMessageDelegate { get; set; }
    }

    public class MessageContext<TTask> : IMessageContext where TTask : ITask
    {
        public MessageContext(
            TTask task,
            int tenantId,
            Guid correlationId,
            Guid causationId,
            IMsmqTransaction msmqTransaction)
        {
            Task = task;
            TenantId = tenantId;
            CorrelationId = correlationId;
            CausationId = causationId;
            MsmqTransaction = msmqTransaction;
        }

        public TTask Task { get; }

        public int TenantId { get; }

        public Guid CorrelationId { get; }

        public Guid CausationId { get; }

        public IMsmqTransaction MsmqTransaction { get; }

        public async Task PublishMessage(ITask task)
        {
            await PublishMessageDelegate(task, TenantId, CausationId, MsmqTransaction);
        }

        public PublishMessageDelegate PublishMessageDelegate { get; set; } = null!;
    }
}
