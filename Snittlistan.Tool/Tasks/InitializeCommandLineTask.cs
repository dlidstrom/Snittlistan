namespace Snittlistan.Tool.Tasks
{
    using System;
    using Snittlistan.Queue;
    using Snittlistan.Queue.Infrastructure;
    using Snittlistan.Queue.Messages;

    public class InitializeCommandLineTask : ICommandLineTask
    {
        public void Run(string[] args)
        {
            if (args.Length != 3)
            {
                throw new Exception("Specify email and password");
            }

            string email = args[1];
            string password = args[2];
            using MsmqGateway.MsmqTransactionScope scope = MsmqGateway.AutoCommitScope();
            foreach (Tenant tenant in CommandLineTaskHelper.Tenants())
            {
                MessageEnvelope envelope = new(
                    new InitializeIndexesTask(email, password),
                    tenant.TenantId,
                    Guid.NewGuid(),
                    null,
                    Guid.NewGuid());
                scope.PublishMessage(envelope);
            }

            scope.Commit();
        }

        public string HelpText => "Initializes indexes and migrates WebsiteConfig for all sites.";
    }
}
