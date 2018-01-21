using Snittlistan.Queue;
using Snittlistan.Queue.Messages;

namespace Snittlistan.Tool.Tasks
{
    public class InitializeCommandLineTask : ICommandLineTask
    {
        public void Run(string[] args)
        {
            using (var scope = MsmqGateway.AutoCommitScope())
            {
                foreach (var url in CommandLineTaskHelper.AllApiUrls())
                {
                    var envelope = new MessageEnvelope(new InitializeIndexesMessage(), url);
                    scope.PublishMessage(envelope);
                }

                scope.Commit();
            }
        }

        public string HelpText => "Initializes indexes and migrates WebsiteConfig for all sites.";
    }
}