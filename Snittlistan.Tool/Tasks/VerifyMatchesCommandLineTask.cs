using Snittlistan.Queue;
using Snittlistan.Queue.Messages;

namespace Snittlistan.Tool.Tasks
{
    public class VerifyMatchesCommandLineTask : ICommandLineTask
    {
        public void Run(string[] args)
        {
            using (var scope = MsmqGateway.AutoCommitScope())
            {
                foreach (var apiUrl in CommandLineTaskHelper.AllApiUrls())
                {
                    scope.PublishMessage(new MessageEnvelope(new VerifyMatchesMessage(), apiUrl));
                }

                scope.Commit();
            }
        }

        public string HelpText => "Verifies registered matches";
    }
}