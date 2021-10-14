#nullable enable

namespace Snittlistan.Web.Models
{
    public class SendEmail : EmailBase
    {
        private SendEmail(string recipient, string subject, string content)
            : base("Mail", recipient, subject)
        {
            Content = content;
        }

        public string Content { get; }

        public static SendEmail ToRecipient(string recipient, string subject, string content)
        {
            return new SendEmail(recipient, subject, content);
        }

        public static SendEmail ToAdmin(string subject, string content)
        {
            return new SendEmail(OwnerEmail, subject, content);
        }
    }
}
