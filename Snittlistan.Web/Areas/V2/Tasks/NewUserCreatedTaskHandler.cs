#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.Areas.V2.Tasks;

public class NewUserCreatedTaskHandler : TaskHandler<NewUserCreatedTask>
{
    public override async Task Handle(MessageContext<NewUserCreatedTask> context)
    {
        string recipient = context.Task.Email;
        const string Subject = "Välkommen till Snittlistan!";
        string activationKey = context.Task.ActivationKey;
        string id = context.Task.UserId;

        UserRegisteredEmail email = new(
            recipient,
            Subject,
            id,
            activationKey);
        await CompositionRoot.EmailService.SendAsync(email);
    }
}
