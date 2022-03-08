#nullable enable

namespace Snittlistan.Queue.Messages;

public class PublishRosterMailTask : TaskBase
{
    public PublishRosterMailTask(
        string rosterId,
        string playerId,
        string recipientEmail,
        string recipientName,
        string replyToEmail,
        Uri rosterLink,
        Uri userProfileLink)
        : base(new(typeof(PublishRosterMailTask).FullName, $"{rosterId}/{recipientEmail}"))
    {
        RosterId = rosterId;
        PlayerId = playerId;
        RecipientEmail = recipientEmail;
        RecipientName = recipientName;
        ReplyToEmail = replyToEmail;
        RosterLink = rosterLink;
        UserProfileLink = userProfileLink;
    }

    public string RosterId { get; }

    public string PlayerId { get; }

    public string RecipientEmail { get; }

    public string RecipientName { get; }

    public string ReplyToEmail { get; }

    public Uri RosterLink { get; }

    public Uri UserProfileLink { get; }
}
