#nullable enable

namespace Snittlistan.Tool.Tasks
{
    using System.Threading.Tasks;
    using Snittlistan.Queue.Commands;

    public class GetRostersFromBitsCommandLineTask : CommandLineTask
    {
        public override async Task Run(string[] args)
        {
            await ExecuteCommand(new GetRostersFromBitsCommand());
        }

        public override string HelpText => "Gets rosters from BITS for the entire club.";
    }
}
