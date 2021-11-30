#nullable enable

namespace Snittlistan.Queue.Messages;
public class MessageEnvelope
{
    public MessageEnvelope(
        object payload,
        int tenantId,
        string hostname,
        Guid correlationId,
        Guid? causationId,
        Guid messageId)
    {
        Payload = payload;
        TenantId = tenantId;
        Hostname = hostname;
        CorrelationId = correlationId;
        CausationId = causationId;
        MessageId = messageId;
    }

    public object Payload { get; }

    public int TenantId { get; }

    public string Hostname { get; }

    public Guid CorrelationId { get; }

    public Guid? CausationId { get; }

    public Guid MessageId { get; }
}
