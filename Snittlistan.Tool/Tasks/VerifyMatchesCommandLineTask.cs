namespace Snittlistan.Tool.Tasks
{
    using Queue;
    using Queue.Messages;

    public class VerifyMatchesCommandLineTask : ICommandLineTask
    {
        public void Run(string[] args)
        {
            using (MsmqGateway.MsmqTransactionScope scope = MsmqGateway.AutoCommitScope())
            {
                foreach (System.Uri apiUrl in CommandLineTaskHelper.AllApiUrls())
                {
                    scope.PublishMessage(new MessageEnvelope(new VerifyMatchesMessage(), apiUrl));
                }

                scope.Commit();
            }
        }

        public string HelpText => "Verifies registered matches";
    }
}