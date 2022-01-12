#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.TaskHandlers;

public class UserInvitedTaskHandler : TaskHandler<UserInvitedTask>
{
    public override async Task Handle(HandlerContext<UserInvitedTask> context)
    {
        string recipient = context.Payload.Email;
        const string Subject = "Välkommen till Snittlistan!";
        string activationUri = context.Payload.ActivationUri;

        InviteUserEmail email = new(
            recipient,
            Subject,
            activationUri);
        await CompositionRoot.EmailService.SendAsync(email);
    }
}
