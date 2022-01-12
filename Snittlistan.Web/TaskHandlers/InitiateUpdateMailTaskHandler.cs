#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Commands;

namespace Snittlistan.Web.TaskHandlers;

public class InitiateUpdateMailTaskHandler
    : TaskHandler<InitiateUpdateMailTask, InitiateUpdateMailCommandHandler.Command>
{
    protected override InitiateUpdateMailCommandHandler.Command CreateCommand(InitiateUpdateMailTask payload)
    {
        return new(payload.RosterId);
    }
}
