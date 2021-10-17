namespace Snittlistan.Tool.Tasks
{
    using Queue;
    using Queue.Messages;

    public class GetRostersFromBitsCommandLineTask : ICommandLineTask
    {
        public void Run(string[] args)
        {
            using (MsmqGateway.MsmqTransactionScope scope = MsmqGateway.AutoCommitScope())
            {
                foreach (System.Uri apiUrl in CommandLineTaskHelper.AllApiUrls())
                {
                    scope.PublishMessage(new MessageEnvelope(new GetRostersFromBitsTask(), apiUrl));
                }

                scope.Commit();
            }
        }

        public string HelpText => "Gets rosters from BITS for the entire club.";
    }
}
