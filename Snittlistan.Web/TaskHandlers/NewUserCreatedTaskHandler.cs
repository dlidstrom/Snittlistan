#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Commands;

namespace Snittlistan.Web.TaskHandlers;

public class NewUserCreatedTaskHandler
    : TaskHandler<NewUserCreatedTask, NewUserCreatedCommandHandler.Command>
{
    protected override NewUserCreatedCommandHandler.Command CreateCommand(NewUserCreatedTask payload)
    {
        return new(payload.Email, payload.ActivationKey, payload.UserId);
    }
}
