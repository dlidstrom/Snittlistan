namespace Snittlistan.Tool.Tasks
{
    using Queue;
    using Queue.Messages;

    public class VerifyMatchesCommandLineTask : ICommandLineTask
    {
        public void Run(string[] args)
        {
            bool force = args.Length == 2 && args[1] == "--force";
            using (MsmqGateway.MsmqTransactionScope scope = MsmqGateway.AutoCommitScope())
            {
                foreach (System.Uri apiUrl in CommandLineTaskHelper.AllApiUrls())
                {
                    scope.PublishMessage(new MessageEnvelope(new VerifyMatchesTask(force), apiUrl));
                }

                scope.Commit();
            }
        }

        public string HelpText => "Verifies registered matches. Supply --force to force all.";
    }
}
