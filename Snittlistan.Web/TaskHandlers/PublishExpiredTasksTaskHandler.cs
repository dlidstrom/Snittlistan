#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Commands;

namespace Snittlistan.Web.TaskHandlers;

public class PublishExpiredTasksTaskHandler
    : TaskHandler<PublishExpiredTasksTask, PublishExpiredTasksCommandHandler.Command>
{
    protected override PublishExpiredTasksCommandHandler.Command CreateCommand(PublishExpiredTasksTask payload)
    {
        return new();
    }
}
