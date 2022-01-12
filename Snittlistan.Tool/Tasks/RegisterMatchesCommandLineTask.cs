#nullable enable

using Snittlistan.Queue.ExternalCommands;

namespace Snittlistan.Tool.Tasks;

public class RegisterMatchesCommandLineTask : CommandLineTask
{
    public override async Task Run(string[] args)
    {
        await ExecuteCommand(new RegisterMatchesCommand());
    }

    public override string HelpText => "Registers matches from Bits";
}
