#nullable enable

using Snittlistan.Queue.ExternalCommands;

namespace Snittlistan.Tool.Tasks;

public class GetPlayersFromBitsCommandLineTask : CommandLineTask
{
    public override async Task Run(string[] args)
    {
        await ExecuteCommand(new GetPlayersFromBitsCommand());
    }

    public override string HelpText => "Gets players from BITS.";
}
