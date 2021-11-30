using Snittlistan.Queue.Commands;

namespace Snittlistan.Tool.Tasks;
public class VerifyMatchesCommandLineTask : CommandLineTask
{
    public override async Task Run(string[] args)
    {
        bool force = args.Length == 2 && args[1] == "--force";
        await ExecuteCommand(new VerifyMatchesCommand(force));
    }

    public override string HelpText => "Verifies registered matches. Supply --force to force all.";
}
