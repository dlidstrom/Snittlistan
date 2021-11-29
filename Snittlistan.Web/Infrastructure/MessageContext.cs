#nullable enable

namespace Snittlistan.Web.Infrastructure
{
    using System;
    using System.Threading.Tasks;
    using Snittlistan.Queue;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Web.Infrastructure.Database;

    public delegate Task PublishMessageDelegate(
        TaskBase task,
        Tenant tenant,
        Guid causationId,
        IMsmqTransaction msmqTransaction);

    public class MessageContext<TTask> : IMessageContext where TTask : TaskBase
    {
        public MessageContext(
            TTask task,
            Tenant tenant,
            Guid correlationId,
            Guid causationId,
            IMsmqTransaction msmqTransaction)
        {
            Task = task;
            Tenant = tenant;
            CorrelationId = correlationId;
            CausationId = causationId;
            MsmqTransaction = msmqTransaction;
        }

        public TTask Task { get; }

        public Tenant Tenant { get; }

        public Guid CorrelationId { get; }

        public Guid CausationId { get; }

        public IMsmqTransaction MsmqTransaction { get; }

        public async Task PublishMessage(TaskBase task)
        {
            await PublishMessageDelegate(task, Tenant, CausationId, MsmqTransaction);
        }

        public PublishMessageDelegate PublishMessageDelegate { get; set; } = null!;
    }
}
