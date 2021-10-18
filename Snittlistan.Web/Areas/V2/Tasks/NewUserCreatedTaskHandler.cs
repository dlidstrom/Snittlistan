#nullable enable

namespace Snittlistan.Web.Areas.V2.Tasks
{
    using System.Threading.Tasks;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Web.Models;

    public class NewUserCreatedTaskHandler : TaskHandler<NewUserCreatedTask>
    {
        public override async Task Handle(NewUserCreatedTask task)
        {
            string recipient = task.Email;
            const string Subject = "Välkommen till Snittlistan!";
            string activationKey = task.ActivationKey;
            string id = task.UserId;

            UserRegisteredEmail email = new(
                recipient,
                Subject,
                id,
                activationKey);
            await EmailService.SendAsync(email);
        }
    }
}
