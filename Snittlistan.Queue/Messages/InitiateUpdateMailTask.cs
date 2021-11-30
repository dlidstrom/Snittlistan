#nullable enable

namespace Snittlistan.Queue.Messages;
public class InitiateUpdateMailTask : TaskBase
{
    public InitiateUpdateMailTask(string rosterId, int _, Guid correlationId)
        : base(new(typeof(InitiateUpdateMailTask), rosterId))
    {
        RosterId = rosterId;
        CorrelationId = correlationId;
    }

    public string RosterId { get; }

    public Guid CorrelationId { get; }
}
