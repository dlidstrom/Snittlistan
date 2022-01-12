#nullable enable

namespace Snittlistan.Queue.ExternalCommands;

public class SendDelayedMailCommand : CommandBase
{
    public SendDelayedMailCommand(
        string recipient,
        int delayInSeconds,
        string hostname,
        string subject,
        string content)
    {
        Recipient = recipient;
        DelayInSeconds = delayInSeconds;
        Hostname = hostname;
        Subject = subject;
        Content = content;
    }

    public string Recipient { get; }

    public int DelayInSeconds { get; }

    public string Hostname { get; }

    public string Subject { get; }

    public string Content { get; }
}
