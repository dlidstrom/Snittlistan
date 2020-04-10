namespace Snittlistan.Tool.Tasks
{
    using Queue;
    using Queue.Messages;

    public class GetRostersFromBitsCommandLineTask : ICommandLineTask
    {
        public void Run(string[] args)
        {
            using (var scope = MsmqGateway.AutoCommitScope())
            {
                foreach (var apiUrl in CommandLineTaskHelper.AllApiUrls())
                {
                    scope.PublishMessage(new MessageEnvelope(new GetRostersFromBitsMessage(), apiUrl));
                }

                scope.Commit();
            }
        }

        public string HelpText => "Gets rosters from BITS for the entire club.";
    }
}
