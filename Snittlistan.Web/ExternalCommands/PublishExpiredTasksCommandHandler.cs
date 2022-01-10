#nullable enable

using Snittlistan.Queue.ExternalCommands;
using Snittlistan.Queue.Messages;

namespace Snittlistan.Web.ExternalCommands;

public class PublishExpiredTasksCommandHandler : CommandHandler<PublishExpiredTasksCommand>
{
    protected override Task<TaskBase> CreateMessage(PublishExpiredTasksCommand command)
    {
        return Task.FromResult((TaskBase)new PublishExpiredTasksTask());
    }
}
