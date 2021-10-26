#nullable enable

namespace Snittlistan.Tool.Tasks
{
    using System;
    using Queue;
    using Queue.Messages;
    using Snittlistan.Queue.Infrastructure;

    public class GetRostersFromBitsCommandLineTask : ICommandLineTask
    {
        public void Run(string[] args)
        {
            using MsmqGateway.MsmqTransactionScope scope = MsmqGateway.AutoCommitScope();
            foreach (Tenant tenant in CommandLineTaskHelper.Tenants())
            {
                MessageEnvelope envelope = new(
                    new GetRostersFromBitsTask(),
                    tenant.TenantId,
                    Guid.NewGuid(),
                    null,
                    Guid.NewGuid());
                scope.PublishMessage(envelope);
            }

            scope.Commit();
        }

        public string HelpText => "Gets rosters from BITS for the entire club.";
    }
}
