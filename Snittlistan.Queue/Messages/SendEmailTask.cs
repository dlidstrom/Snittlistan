#nullable enable

using System.Text;
using Newtonsoft.Json;

namespace Snittlistan.Queue.Messages;

public class SendEmailTask : TaskBase
{
    [JsonConstructor]
    private SendEmailTask(
        string to,
        string replyTo,
        string subject,
        string content,
        int ratePerSeconds)
        : base(new(typeof(SendEmailTask).FullName, to))
    {
        To = to;
        ReplyTo = replyTo;
        Subject = subject;
        Content = content;
        RatePerSeconds = ratePerSeconds;
    }

    public static SendEmailTask Create(
        string recipient,
        string replyTo,
        string subject,
        string content,
        int ratePerSeconds)
    {
        SendEmailTask emailTask =
            new(
                recipient,
                replyTo,
                Convert.ToBase64String(Encoding.UTF8.GetBytes(subject)),
                Convert.ToBase64String(Encoding.UTF8.GetBytes(content)),
                ratePerSeconds);
        return emailTask;
    }

    public string To { get; }

    public string ReplyTo { get; }

    public string Subject { get; }

    public string Content { get; }

    public int RatePerSeconds { get; }

    public override string ToString()
    {
        StringBuilder builder = new();
        return builder
            .AppendLine("Subject:   " + Subject)
            .AppendLine("Recipient: " + To)
            .AppendLine("Content:   " + Encoding.UTF8.GetString(Convert.FromBase64String(Content)).Substring(0, Math.Min(Content.Length, 250)))
            .AppendLine("Rate: " + To)
            .ToString();
    }
}
