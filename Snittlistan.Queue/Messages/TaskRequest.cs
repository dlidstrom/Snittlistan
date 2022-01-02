#nullable enable

using Newtonsoft.Json;

namespace Snittlistan.Queue.Messages;

public class TaskRequest
{
    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        Formatting = Formatting.Indented,
        TypeNameHandling = TypeNameHandling.All,
        MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead
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
