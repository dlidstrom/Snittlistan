#nullable enable

namespace Snittlistan.Queue.Messages
{
    using System;

    public interface IMessageContext
    {
        Action<ITask> PublishMessage { get; set; }
    }

    public class MessageContext<TTask> : IMessageContext where TTask : ITask
    {
        public MessageContext(
            TTask task,
            Guid correlationId,
            Guid causationId)
        {
            Task = task;
            CorrelationId = correlationId;
            CausationId = causationId;
        }

        public TTask Task { get; }

        public Guid CorrelationId { get; }

        public Guid CausationId { get; }

        public Action<ITask> PublishMessage { get; set; } = null!;
    }
}
