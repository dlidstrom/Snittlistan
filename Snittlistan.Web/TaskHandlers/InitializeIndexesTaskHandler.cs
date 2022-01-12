#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Commands;

namespace Snittlistan.Web.TaskHandlers;

public class InitializeIndexesTaskHandler
    : TaskHandler<InitializeIndexesTask, InitializeIndexesCommandHandler.Command>
{
    protected override InitializeIndexesCommandHandler.Command CreateCommand(InitializeIndexesTask payload)
    {
        return new(payload.Email, payload.Password);
    }
}
