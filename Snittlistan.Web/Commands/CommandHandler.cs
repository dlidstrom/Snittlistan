#nullable enable

namespace Snittlistan.Web.Commands
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Snittlistan.Queue;
    using Snittlistan.Queue.Commands;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Web.Infrastructure;
    using Snittlistan.Web.Infrastructure.Database;

    public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : CommandBase
    {
        public Databases Databases { get; set; } = null!;

        public IMsmqTransaction MsmqTransaction { get; set; } = null!;

        public async Task Handle(TCommand command)
        {
            object task = await CreateMessage(command);
            var query =
                    from tenant in Databases.Snittlistan.Tenants
                    select new
                    {
                        tenant.TenantId,
                        tenant.Hostname
                    };
            var tenants = await query.ToArrayAsync();
            foreach (var tenant in tenants)
            {
                MessageEnvelope envelope = new(
                    task,
                    tenant.TenantId,
                    tenant.Hostname,
                    command.CorrelationId,
                    null,
                    Guid.NewGuid());
                MsmqTransaction.PublishMessage(envelope);
            }
        }

        protected abstract Task<object> CreateMessage(TCommand command);
    }
}
