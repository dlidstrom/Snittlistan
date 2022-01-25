#nullable enable

namespace Snittlistan.Queue.Messages;

public class PublishRosterMailTask : TaskBase
{
    public PublishRosterMailTask(string rosterId, string playerId)
        : base(new(typeof(PublishRosterMailTask).FullName, $"{rosterId}/{playerId}"))
    {
        RosterId = rosterId;
        PlayerId = playerId;
    }

    public string RosterId { get; }

    public string PlayerId { get; }
}
