#nullable enable

namespace Snittlistan.Web.Areas.V2.Tasks
{
    using System.Threading.Tasks;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Web.Models;

    public class UserInvitedTaskHandler : TaskHandler<UserInvitedTask>
    {
        public override async Task Handle(UserInvitedTask task)
        {
            string recipient = task.Email;
            const string Subject = "Välkommen till Snittlistan!";
            string activationUri = task.ActivationUri;

            InviteUserEmail email = new(
                recipient,
                Subject,
                activationUri);
            await EmailService.SendAsync(email);
        }
    }
}
