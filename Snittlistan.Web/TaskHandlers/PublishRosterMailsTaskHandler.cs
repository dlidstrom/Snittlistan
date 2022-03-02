#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Commands;

namespace Snittlistan.Web.TaskHandlers;

public class PublishRosterMailsTaskHandler
    : TaskHandler<PublishRosterMailsTask, PublishRosterMailsCommandHandler.Command>
{
    protected override PublishRosterMailsCommandHandler.Command CreateCommand(PublishRosterMailsTask payload)
    {
        return new(
            payload.RosterId,
            payload.RosterLink,
            payload.UserProfileLink);
    }
}
