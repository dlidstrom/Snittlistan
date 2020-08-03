#pragma warning disable 618
namespace Snittlistan.Queue.Messages
{
    using System;
    using System.Text;

    public class EmailTask
    {
        [Obsolete("Use the factory method")]
        public EmailTask(string recipient, string subject, string content)
        {
            Recipient = recipient;
            Subject = subject;
            Content = content;
        }

        public static EmailTask Create(string recipient, string subject, string content)
        {
            if (subject == null) throw new ArgumentNullException(nameof(subject));
            if (content == null) throw new ArgumentNullException(nameof(content));
            var emailTask =
                new EmailTask(
                    recipient,
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(subject)),
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(content)));
            return emailTask;
        }

        public string Recipient { get; }

        public string Subject { get; }

        public string Content { get; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine("Subject:   " + Subject);
            builder.AppendLine("Recipient: " + Recipient);
            builder.AppendLine("Content:   " + Encoding.UTF8.GetString(Convert.FromBase64String(Content)).Substring(0, Math.Min(Content.Length, 250)));
            return builder.ToString();
        }
    }
}