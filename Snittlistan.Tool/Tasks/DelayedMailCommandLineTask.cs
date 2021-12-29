#nullable enable

using Snittlistan.Queue.Commands;

namespace Snittlistan.Tool.Tasks;

public class DelayedMailCommandLineTask : CommandLineTask
{
    public override string HelpText => "Send a mail after some delay";

    public override async Task Run(string[] args)
    {
        if (args.Length != 6)
        {
            throw new Exception("Specify recipient, delay (in seconds), hostname, subject, and content");
        }

        string recipient = args[1];
        int delayInSeconds = int.Parse(args[2]);
        string hostname = args[3];
        string subject = args[4];
        string content = args[5];
        SendDelayedMailCommand command = new(
            recipient,
            delayInSeconds,
            hostname,
            subject,
            content);
        await ExecuteCommand(command);
    }
}
