using System;
using System.Text;

namespace Snittlistan.Queue.Messages
{
    public class EmailTask
    {
        public EmailTask(string recipient, string subject, string content)
        {
            Recipient = recipient ?? throw new ArgumentNullException(nameof(recipient));
            Subject = subject ?? throw new ArgumentNullException(nameof(subject));
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }

        public string Recipient { get; }

        public string Subject { get; }

        public string Content { get; }

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