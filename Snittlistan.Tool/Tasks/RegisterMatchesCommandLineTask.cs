using Snittlistan.Queue.Commands;

#nullable enable

namespace Snittlistan.Tool.Tasks;
public class RegisterMatchesCommandLineTask : CommandLineTask
{
    public override async Task Run(string[] args)
    {
        await ExecuteCommand(new RegisterMatchesCommand());
    }

    public override string HelpText => "Registers matches from Bits";
}
