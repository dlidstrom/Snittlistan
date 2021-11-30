#nullable enable

namespace Snittlistan.Web.Commands
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Snittlistan.Queue.Commands;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Web.Areas.V2.Tasks;
    using Snittlistan.Web.Infrastructure.Database;

    public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : CommandBase
    {
        public Databases Databases { get; set; } = null!;

        public async Task Handle(TCommand command)
        {
            TaskBase task = await CreateMessage(command);
            IQueryable<Tenant> query =
                from tenant in Databases.Snittlistan.Tenants
                select tenant;
            Tenant[] tenants = await query.ToArrayAsync();
            foreach (Tenant tenant in tenants)
            {
                TaskPublisher taskPublisher = new(tenant, Databases, command.CorrelationId, null);
                MessageEnvelope envelope = new(
                    task,
                    tenant.TenantId,
                    tenant.Hostname,
                    command.CorrelationId,
                    null,
                    Guid.NewGuid());
                taskPublisher.PublishTask(task, "system");
            }
        }

        protected abstract Task<TaskBase> CreateMessage(TCommand command);
    }
}
