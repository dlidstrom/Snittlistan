using Snittlistan.Queue.Commands;

namespace Snittlistan.Tool.Tasks;

public class InitializeCommandLineTask : CommandLineTask
{
    public override async Task Run(string[] args)
    {
        if (args.Length != 3)
        {
            throw new Exception("Specify email and password");
        }

        string email = args[1];
        string password = args[2];
        await ExecuteCommand(new InitializeIndexesCommand(email, password));
    }

    public override string HelpText => "Initializes indexes and migrates WebsiteConfig for all sites.";
}
