#nullable enable

namespace Snittlistan.Tool.Tasks
{
    using System.Threading.Tasks;
    using Snittlistan.Queue.Commands;

    public class GetPlayersFromBitsCommandLineTask : CommandLineTask
    {
        public override async Task Run(string[] args)
        {
            await ExecuteCommand(new GetPlayersFromBitsCommand());
        }

        public override string HelpText => "Gets players from BITS.";
    }
}
