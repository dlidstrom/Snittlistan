#nullable enable

namespace Snittlistan.Queue.Messages;

public class TaskRequest
{
    public TaskRequest(MessageEnvelope envelope)
    {
        TaskJson = envelope.Payload.ToJson();
        CorrelationId = envelope.CorrelationId;
        MessageId = envelope.MessageId;
    }

    public int TenantId { get; }

    public string TaskJson { get; }

    public Guid CorrelationId { get; }

    public Guid MessageId { get; }
}
