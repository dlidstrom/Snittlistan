#nullable enable

using Snittlistan.Queue.Commands;
using Snittlistan.Queue.Messages;

namespace Snittlistan.Web.Commands;

public class PublishExpiredTasksCommandHandler : CommandHandler<PublishExpiredTasksCommand>
{
    protected override Task<TaskBase> CreateMessage(PublishExpiredTasksCommand command)
    {
        return Task.FromResult((TaskBase)new PublishExpiredTasksTask());
    }
}
