namespace Snittlistan.Queue.Messages
{
    using System;
    using System.Text;
    using Newtonsoft.Json;

    public class EmailTask : ITask
    {
        [JsonConstructor]
        private EmailTask(string to, string subject, string content)
        {
            To = to;
            Subject = subject;
            Content = content;
        }

        public static EmailTask Create(string recipient, string subject, string content)
        {
            if (subject == null)
            {
                throw new ArgumentNullException(nameof(subject));
            }

            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            EmailTask emailTask =
                new(
                    recipient,
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(subject)),
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(content)));
            return emailTask;
        }

        public string To { get; }

        public string Subject { get; }

        public string Content { get; }

        public BusinessKey BusinessKey => new(GetType(), To);

        public override string ToString()
        {
            StringBuilder builder = new();
            return builder
                .AppendLine("Subject:   " + Subject)
                .AppendLine("Recipient: " + To)
                .AppendLine("Content:   " + Encoding.UTF8.GetString(Convert.FromBase64String(Content)).Substring(0, Math.Min(Content.Length, 250)))
                .ToString();
        }
    }
}
