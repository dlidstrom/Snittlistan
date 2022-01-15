#nullable enable

namespace Snittlistan.Queue.Messages;

public class PublishRosterMailsTask : TaskBase
{
    public PublishRosterMailsTask(string rosterId)
        : base(new(typeof(PublishRosterMailsTask).FullName, rosterId))
    {
        RosterId = rosterId;
    }

    public string RosterId { get; }
}
