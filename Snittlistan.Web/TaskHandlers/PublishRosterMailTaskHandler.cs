#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Commands;

namespace Snittlistan.Web.TaskHandlers;

public class PublishRosterMailTaskHandler
    : TaskHandler<PublishRosterMailTask, PublishRosterMailCommandHandler.Command>
{
    protected override PublishRosterMailCommandHandler.Command CreateCommand(PublishRosterMailTask payload)
    {
        return new(payload.RosterId, payload.PlayerId);
    }
}
