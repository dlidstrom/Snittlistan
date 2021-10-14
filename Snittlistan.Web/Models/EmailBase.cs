#nullable enable

namespace Snittlistan.Web.Models
{
    using System.Configuration;
    using Postal;

    public abstract class EmailBase : Email
    {
        protected EmailBase(string viewName, string recipient, string subject)
            : base(viewName)
        {
            Recipient = recipient;
            Subject = subject;
        }

        public static string OwnerEmail { get; } = ConfigurationManager.AppSettings["OwnerEmail"];

        public string Bcc { get; protected set; } = OwnerEmail;
        public string From { get; protected set; } = OwnerEmail;
        public string Recipient { get; }
        public string Subject { get; }
    }
}
