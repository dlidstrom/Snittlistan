namespace Snittlistan.Tool.Tasks
{
    using System;
    using Snittlistan.Queue;
    using Snittlistan.Queue.Messages;

    public class RegisterMatchesCommandLineTask : ICommandLineTask
    {
        public void Run(string[] args)
        {
            using (MsmqGateway.MsmqTransactionScope scope = MsmqGateway.AutoCommitScope())
            {
                foreach (Uri apiUrl in CommandLineTaskHelper.AllApiUrls())
                {
                    scope.PublishMessage(new MessageEnvelope(new RegisterMatchesMessage(), apiUrl));
                }

                scope.Commit();
            }
        }

        public string HelpText => "Registers matches from Bits";
    }
}
