#nullable enable

namespace Snittlistan.Web.Models
{
    using System.Configuration;
    using Postal;

    public abstract class EmailBase : Email
    {
        protected EmailBase(string viewName, string to, string subject)
            : base(viewName)
        {
            To = to;
            Subject = subject;
        }

        public static string OwnerEmail { get; } = ConfigurationManager.AppSettings["OwnerEmail"];

        public string Bcc { get; protected set; } = OwnerEmail;
        public string From { get; protected set; } = OwnerEmail;
        public string To { get; }
        public string Subject { get; }
    }
}
