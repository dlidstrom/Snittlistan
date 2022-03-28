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
        int rate,
        int perSeconds,
        string key)
        : base(new(typeof(SendEmailTask).FullName, key))
    {
        To = to;
        ReplyTo = replyTo;
        Subject = subject;
        Content = content;
        Rate = rate;
        PerSeconds = perSeconds;
    }

    public static SendEmailTask Create(
        string recipient,
        string replyTo,
        string subject,
        string content,
        int rate,
        int perSeconds,
        string key)
    {
        SendEmailTask emailTask =
            new(
                recipient,
                replyTo,
                Convert.ToBase64String(Encoding.UTF8.GetBytes(subject)),
                Convert.ToBase64String(Encoding.UTF8.GetBytes(content)),
                rate,
                perSeconds,
                key);
        return emailTask;
    }

    public string To { get; }

    public string ReplyTo { get; }

    public string Subject { get; }

    public string Content { get; }

    public int Rate { get; }

    public int PerSeconds { get; }
}
