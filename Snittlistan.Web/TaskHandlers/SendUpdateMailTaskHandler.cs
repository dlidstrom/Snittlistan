#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Commands;

namespace Snittlistan.Web.TaskHandlers;

public class SendUpdateMailTaskHandler
    : TaskHandler<SendUpdateMailTask, SendUpdateMailCommandHandler.Command>
{
    protected override SendUpdateMailCommandHandler.Command CreateCommand(SendUpdateMailTask payload)
    {
        return new(payload.RosterId, payload.PlayerId);
    }
}
