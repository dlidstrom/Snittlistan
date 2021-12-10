#nullable enable

namespace Snittlistan.Web.Models;

public class SendEmail : EmailBase
{
    private SendEmail(string to, string subject, string content)
        : base("Mail", to, subject)
    {
        Content = content;
    }

    public string Content { get; }

    public static SendEmail ToRecipient(string to, string subject, string content)
    {
        return new SendEmail(to, subject, content);
    }

    public static SendEmail ToAdmin(string to, string content)
    {
        return new SendEmail(OwnerEmail, to, content);
    }
}
