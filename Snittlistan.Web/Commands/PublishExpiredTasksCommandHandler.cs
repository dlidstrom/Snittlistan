#nullable enable

using Snittlistan.Queue.Commands;
using Snittlistan.Queue.Messages;
using Snittlistan.Web.Areas.V2.Tasks;

namespace Snittlistan.Web.Commands;

public class PublishExpiredTasksCommandHandler : CommandHandler<PublishExpiredTasksCommand>
{
    protected override Task<TaskBase> CreateMessage(PublishExpiredTasksCommand command)
    {
        return Task.FromResult((TaskBase)new PublishExpiredTasksTaskHandler.PublishExpiredTasksTask());
    }
}
