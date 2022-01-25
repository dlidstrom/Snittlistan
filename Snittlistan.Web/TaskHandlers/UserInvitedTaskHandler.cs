#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Commands;

namespace Snittlistan.Web.TaskHandlers;

public class UserInvitedTaskHandler
    : TaskHandler<UserInvitedTask, UserInvitedCommandHandler.Command>
{
    protected override UserInvitedCommandHandler.Command CreateCommand(UserInvitedTask payload)
    {
        return new(payload.ActivationUri, payload.Email);
    }
}
