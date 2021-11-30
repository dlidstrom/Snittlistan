using Snittlistan.Queue.Messages;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Models;

#nullable enable

namespace Snittlistan.Web.Areas.V2.Tasks;
public class UserInvitedTaskHandler : TaskHandler<UserInvitedTask>
{
    public override async Task Handle(MessageContext<UserInvitedTask> context)
    {
        string recipient = context.Task.Email;
        const string Subject = "Välkommen till Snittlistan!";
        string activationUri = context.Task.ActivationUri;

        InviteUserEmail email = new(
            recipient,
            Subject,
            activationUri);
        await EmailService.SendAsync(email);
    }
}
