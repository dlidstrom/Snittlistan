#nullable enable

namespace Snittlistan.Queue.Messages;

public class PublishRosterMailTask : TaskBase
{
    public PublishRosterMailTask(
        string rosterId,
        string playerId,
        string replyToEmail,
        Uri rosterLink)
        : base(new(typeof(PublishRosterMailTask).FullName, $"{rosterId}/{playerId}"))
    {
        RosterId = rosterId;
        PlayerId = playerId;
        ReplyToEmail = replyToEmail;
        RosterLink = rosterLink;
    }

    public string RosterId { get; }

    public string PlayerId { get; }

    public string ReplyToEmail { get; }

    public Uri RosterLink { get; }
}
