#nullable enable

namespace Snittlistan.Queue.Messages;

public class PublishRosterMailsTask : TaskBase
{
    public PublishRosterMailsTask(
        string rosterId,
        Uri rosterLink)
        : base(new(typeof(PublishRosterMailsTask).FullName, rosterId))
    {
        RosterId = rosterId;
        RosterLink = rosterLink;
    }

    public string RosterId { get; }

    public Uri RosterLink { get; }
}
