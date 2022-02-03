#nullable enable

using Postal;

namespace Snittlistan.Web.Models;

public class SendEmail_State : EmailState
{
    public SendEmail_State(string to, string subject, string content)
        : base(OwnerEmail, to, OwnerEmail, subject)
    {
        Content = content;
    }

    public string Content { get; }

    public override Email CreateEmail()
    {
        return new SendEmail(To, Subject, Content);
    }
}
