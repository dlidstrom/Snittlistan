#nullable enable

namespace Snittlistan.Web.Models;

public class SendEmail : EmailBase
{
    private readonly SendEmail_State _state;

    public SendEmail(
        string to,
        string replyTo,
        string subject,
        string content)
        : base("Mail")
    {
        _state = new(
            to,
            replyTo,
            subject,
            content);
    }

    public string ReplyTo => _state.ReplyTo;

    public string Content => _state.Content;

    public override EmailState State => _state;

    public static SendEmail ToRecipient(
        string to,
        string replyTo,
        string subject,
        string content)
    {
        return new SendEmail(
            to,
            replyTo,
            subject,
            content);
    }

    public static SendEmail ToAdmin(string subject, string content)
    {
        return new SendEmail(
            EmailState.OwnerEmail,
            string.Empty,
            subject,
            content);
    }
}
