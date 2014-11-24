using System;
using System.Text;

namespace Snittlistan.Web.Tasks
{
    public class EmailTask
    {
        public EmailTask(string recipient, string subject, string content)
        {
            if (recipient == null) throw new ArgumentNullException("recipient");
            if (subject == null) throw new ArgumentNullException("subject");
            if (content == null) throw new ArgumentNullException("content");
            Recipient = recipient;
            Subject = subject;
            Content = content;
        }

        public string Recipient { get; private set; }

        public string Subject { get; private set; }

        public string Content { get; private set; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine("Subject:   " + Subject);
            builder.AppendLine("Recipient: " + Recipient);
            builder.AppendLine("Content:   " + Content.Substring(0, Math.Min(Content.Length, 250)));
            return builder.ToString();
        }
    }
}