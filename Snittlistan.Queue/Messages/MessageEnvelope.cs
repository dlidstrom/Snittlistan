#nullable enable

namespace Snittlistan.Queue.Messages
{
    using System;

    public class MessageEnvelope
    {
        public MessageEnvelope(
            object payload,
            int tenantId,
            Guid correlationId,
            Guid? causationId,
            Guid messageId)
        {
            Payload = payload;
            TenantId = tenantId;
            CorrelationId = correlationId;
            CausationId = causationId;
            MessageId = messageId;
        }

        public object Payload { get; }

        public int TenantId { get; }

        public Guid CorrelationId { get; }

        public Guid? CausationId { get; }

        public Guid MessageId { get; }
    }
}
