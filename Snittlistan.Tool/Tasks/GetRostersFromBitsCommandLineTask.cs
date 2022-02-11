#nullable enable

using Snittlistan.Queue.ExternalCommands;

namespace Snittlistan.Tool.Tasks;

public class GetRostersFromBitsCommandLineTask : CommandLineTask
{
    public override async Task Run(string[] args)
    {
        await ExecuteCommand(new GetRostersFromBitsCommand());
    }

    public override string HelpText => "Gets rosters from BITS for the entire club.";
}
