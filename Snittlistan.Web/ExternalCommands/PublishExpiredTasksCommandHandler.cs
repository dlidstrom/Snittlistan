#nullable enable

using Snittlistan.Queue.ExternalCommands;
using Snittlistan.Queue.Messages;

namespace Snittlistan.Web.ExternalCommands;

public class PublishExpiredTasksCommandHandler
    : CommandHandler<PublishExpiredTasksCommand, PublishExpiredTasksTask>
{
    protected override PublishExpiredTasksTask CreateMessage(PublishExpiredTasksCommand command)
    {
        return new PublishExpiredTasksTask();
    }
}
