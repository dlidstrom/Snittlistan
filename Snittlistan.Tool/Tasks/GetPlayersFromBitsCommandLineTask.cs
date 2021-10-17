namespace Snittlistan.Tool.Tasks
{
    using System;
    using Snittlistan.Queue;
    using Snittlistan.Queue.Messages;

    public class GetPlayersFromBitsCommandLineTask : ICommandLineTask
    {
        public void Run(string[] args)
        {
            using (MsmqGateway.MsmqTransactionScope scope = MsmqGateway.AutoCommitScope())
            {
                foreach (Uri apiUrl in CommandLineTaskHelper.AllApiUrls())
                {
                    scope.PublishMessage(new MessageEnvelope(new GetPlayersFromBitsTask(), apiUrl));
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
