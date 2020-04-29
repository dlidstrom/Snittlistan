namespace Snittlistan.Tool.Tasks
{
    using Queue;
    using Queue.Messages;

    public class GetPlayersFromBitsCommandLineTask : ICommandLineTask
    {
        public void Run(string[] args)
        {
            using (var scope = MsmqGateway.AutoCommitScope())
            {
                foreach (var apiUrl in CommandLineTaskHelper.AllApiUrls())
                {
                    scope.PublishMessage(new MessageEnvelope(new GetPlayersFromBitsMessage(), apiUrl));
                }

                scope.Commit();
            }
        }

        public string HelpText
        {
            get { return "Gets players from BITS."; }
        }
    }
}