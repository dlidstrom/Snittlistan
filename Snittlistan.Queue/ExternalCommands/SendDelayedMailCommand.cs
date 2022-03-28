#nullable enable

namespace Snittlistan.Queue.ExternalCommands;

public class SendDelayedMailCommand : CommandBase
{
    public SendDelayedMailCommand(
        string recipient,
        string replyTo,
        int delayInSeconds,
        string hostname,
        string subject,
        string content,
        int rate,
        int perSeconds)
    {
        Recipient = recipient;
        ReplyTo = replyTo;
        DelayInSeconds = delayInSeconds;
        Hostname = hostname;
        Subject = subject;
        Content = content;
        Rate = rate;
        PerSeconds = perSeconds;
    }

    public string Recipient { get; }

    public string ReplyTo { get; }

    public int DelayInSeconds { get; }

    public string Hostname { get; }

    public string Subject { get; }

    public string Content { get; }

    public int Rate { get; }

    public int PerSeconds { get; }
}
