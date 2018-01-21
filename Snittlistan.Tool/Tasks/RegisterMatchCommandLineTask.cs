using Snittlistan.Queue;
using Snittlistan.Queue.Messages;

namespace Snittlistan.Tool.Tasks
{
    public class RegisterMatchCommandLineTask : ICommandLineTask
    {
        public void Run(string[] args)
        {
            using (var scope = MsmqGateway.AutoCommitScope())
            {
                foreach (var tuple in CommandLineTaskHelper.AllConnectionStrings())
                {
                    scope.PublishMessage(new MessageEnvelope(new RegisterMatchesMessage(), tuple.Item2));
                }

                scope.Commit();
            }
        }

        public string HelpText => "Registers matches from Bits";
    }
}