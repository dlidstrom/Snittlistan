#nullable enable

using System.Data.Entity;
using Snittlistan.Queue.Commands;
using Snittlistan.Queue.Messages;
using Snittlistan.Web.Areas.V2.Tasks;
using Snittlistan.Web.Infrastructure.Database;

namespace Snittlistan.Web.Commands;

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
            taskPublisher.PublishTask(task, "system");
        }
    }

    protected abstract Task<TaskBase> CreateMessage(TCommand command);
}
