#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Commands;

namespace Snittlistan.Web.TaskHandlers;

public class RegisterMatchesTaskHandler
    : TaskHandler<RegisterMatchesTask, RegisterMatchesCommandHandler.Command>
{
    protected override RegisterMatchesCommandHandler.Command CreateCommand(RegisterMatchesTask payload)
    {
        return new();
    }
}
