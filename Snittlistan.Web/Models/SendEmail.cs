#nullable enable

namespace Snittlistan.Web.Models;

public class SendEmail : EmailBase
{
    private readonly SendEmail_State _state;

    public SendEmail(string to, string subject, string content)
        : base("Mail")
    {
        _state = new(
            to,
            subject,
            content);
    }

    public string Content => _state.Content;

    public override EmailState State => _state;

    public static SendEmail ToRecipient(string to, string subject, string content)
    {
        return new SendEmail(to, subject, content);
    }

    public static SendEmail ToAdmin(string to, string content)
    {
        return new SendEmail(EmailState.OwnerEmail, to, content);
    }
}
