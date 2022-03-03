#nullable enable

namespace Snittlistan.Queue.Messages;

public class PublishRosterMailsTask : TaskBase
{
    public PublishRosterMailsTask(
        string rosterId,
        Uri rosterLink,
        Uri userProfileLink)
        : base(new(typeof(PublishRosterMailsTask).FullName, rosterId))
    {
        RosterId = rosterId;
        RosterLink = rosterLink;
        UserProfileLink = userProfileLink;
    }

    public string RosterId { get; }

    public Uri RosterLink { get; }

    public Uri UserProfileLink { get; }
}
