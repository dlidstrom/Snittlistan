#nullable enable

using Postal;

namespace Snittlistan.Web.Models;

public class SendEmail_State : EmailState
{
    public SendEmail_State(
        string to,
        string replyTo,
        string subject,
        string content)
        : base(OwnerEmail, to, OwnerEmail, subject)
    {
        ReplyTo = replyTo;
        Content = content;
    }

    public string ReplyTo { get; }

    public string Content { get; }

    public override Email CreateEmail()
    {
        return new SendEmail(
            To,
            ReplyTo,
            Subject,
            Content);
    }
}
