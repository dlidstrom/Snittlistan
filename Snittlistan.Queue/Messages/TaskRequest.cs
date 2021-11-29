#nullable enable

namespace Snittlistan.Queue.Messages
{
    using System;
    using Newtonsoft.Json;

    public class TaskRequest
    {
        private static readonly JsonSerializerSettings SerializerSettings = new()
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.All
        };

        public TaskRequest(MessageEnvelope envelope)
        {
            TaskJson = JsonConvert.SerializeObject(envelope.Payload, SerializerSettings);
            CorrelationId = envelope.CorrelationId;
            MessageId = envelope.MessageId;
        }

        public int TenantId { get; }

        public string TaskJson { get; }

        public Guid CorrelationId { get; }

        public Guid MessageId { get; }
    }
}
