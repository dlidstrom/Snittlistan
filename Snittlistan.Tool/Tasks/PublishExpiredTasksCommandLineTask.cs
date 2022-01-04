#nullable enable

using Snittlistan.Queue.Commands;

namespace Snittlistan.Tool.Tasks;

public class PublishExpiredTasksCommandLineTask : CommandLineTask
{
    public override string HelpText => "Publishes expired tasks";

    public override async Task Run(string[] args)
    {
        await ExecuteCommand(new PublishExpiredTasksCommand());
    }
}
