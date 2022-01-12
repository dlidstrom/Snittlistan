#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Commands;

namespace Snittlistan.Web.TaskHandlers;

public class RegisterMatchTaskHandler
    : TaskHandler<RegisterPendingMatchTask, RegisterPendingMatchCommandHandler.Command>
{
    protected override RegisterPendingMatchCommandHandler.Command CreateCommand(RegisterPendingMatchTask payload)
    {
        throw new NotImplementedException();
    }
}
