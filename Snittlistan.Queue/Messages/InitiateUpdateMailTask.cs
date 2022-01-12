#nullable enable

namespace Snittlistan.Queue.Messages;

public class InitiateUpdateMailTask : TaskBase
{
    public InitiateUpdateMailTask(string rosterId)
        : base(new(typeof(InitiateUpdateMailTask).FullName, rosterId))
    {
        RosterId = rosterId;
    }

    public string RosterId { get; }
}
