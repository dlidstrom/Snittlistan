namespace Snittlistan.Tool.Tasks
{
    using System;
    using Queue;
    using Queue.Messages;
    using Snittlistan.Queue.Infrastructure;

    public class VerifyMatchesCommandLineTask : ICommandLineTask
    {
        public void Run(string[] args)
        {
            bool force = args.Length == 2 && args[1] == "--force";
            using MsmqGateway.MsmqTransactionScope scope = MsmqGateway.AutoCommitScope();
            foreach (Tenant tenant in CommandLineTaskHelper.Tenants())
            {
                MessageEnvelope envelope = new(
                    new VerifyMatchesTask(force),
                    tenant.TenantId,
                    Guid.NewGuid(),
                    null,
                    Guid.NewGuid());
                scope.PublishMessage(envelope);
            }

            scope.Commit();
        }

        public string HelpText => "Verifies registered matches. Supply --force to force all.";
    }
}
