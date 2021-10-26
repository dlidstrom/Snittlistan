namespace Snittlistan.Tool.Tasks
{
    using System;
    using Snittlistan.Queue;
    using Snittlistan.Queue.Infrastructure;
    using Snittlistan.Queue.Messages;

    public class GetPlayersFromBitsCommandLineTask : ICommandLineTask
    {
        public void Run(string[] args)
        {
            using MsmqGateway.MsmqTransactionScope scope = MsmqGateway.AutoCommitScope();
            foreach (Tenant tenant in CommandLineTaskHelper.Tenants())
            {
                MessageEnvelope envelope = new(
                    new GetPlayersFromBitsTask(),
                    tenant.TenantId,
                    Guid.NewGuid(),
                    null,
                    Guid.NewGuid());
                scope.PublishMessage(envelope);
            }

            scope.Commit();
        }

        public string HelpText => "Gets players from BITS.";
    }
}
