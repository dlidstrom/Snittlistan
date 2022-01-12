#nullable enable

namespace Snittlistan.Queue.Messages;

public class SendUpdateMailTask : TaskBase
{
    public SendUpdateMailTask(string rosterId, string playerId)
        : base(new(typeof(SendUpdateMailTask).FullName, $"{rosterId}/{playerId}"))
    {
        RosterId = rosterId;
        PlayerId = playerId;
    }

    public string RosterId { get; }

    public string PlayerId { get; }
}
