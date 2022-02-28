#nullable enable

using Castle.Core.Logging;
using Snittlistan.Queue;
using Snittlistan.Queue.ExternalCommands;
using Snittlistan.Queue.Messages;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Database;
using System.Data.Entity;

namespace Snittlistan.Web.ExternalCommands;

public class PublishExpiredTasksCommandHandler
    : ICommandHandler<PublishExpiredTasksCommand>
{
    public Databases Databases { get; set; } = null!;

    public ILogger Logger { get; set; } = NullLogger.Instance;

    public MsmqFactory MsmqFactory { get; set; } = null!;

    public async Task Handle(PublishExpiredTasksCommand command)
    {
        DateTime after = DateTime.Now.AddMinutes(-1);
        IQueryable<PublishedTask> query =
            from task in Databases.Snittlistan.PublishedTasks.Include(x => x.Tenant)
            where task.PublishDate < after && task.HandledDate == null
            select task;
        PublishedTask[] expiredTasks = await query.ToArrayAsync();
        if (expiredTasks.Length == 0)
        {
            return;
        }

        using MsmqGateway msmqGateway = MsmqFactory.Create();
        using MsmqGateway.MsmqTransactionScope scope = msmqGateway.Create();
        foreach (PublishedTask publishedTask in expiredTasks)
        {
            MessageEnvelope message = new(
                publishedTask.Task,
                publishedTask.TenantId,
                publishedTask.Tenant.Hostname,
                publishedTask.CorrelationId,
                publishedTask.CausationId,
                publishedTask.MessageId);
            scope.Send(message);
        }

        scope.Commit();
        Logger.InfoFormat("published {length} expired messages", expiredTasks.Length);
    }
}
