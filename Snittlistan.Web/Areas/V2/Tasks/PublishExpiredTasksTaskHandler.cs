#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Database;

namespace Snittlistan.Web.Areas.V2.Tasks;

public class PublishExpiredTasksTaskHandler : TaskHandler<PublishExpiredTasksTaskHandler.PublishExpiredTasksTask>
{
    public override Task Handle(MessageContext<PublishExpiredTasksTask> context)
    {
        DateTime now = DateTime.Now;
        IQueryable<DelayedTask> query =
            from task in Databases.Snittlistan.DelayedTasks
            where task.PublishDate > now
            select task;
        foreach (DelayedTask task in query)
        {
            context.PublishMessage(task.Task);
            task.MarkAsPublished(now);
        }

        return Task.CompletedTask;
    }

    public class PublishExpiredTasksTask : TaskBase
    {
        public PublishExpiredTasksTask()
            : base(new(typeof(PublishExpiredTasksTask), string.Empty))
        {
        }
    }
}
