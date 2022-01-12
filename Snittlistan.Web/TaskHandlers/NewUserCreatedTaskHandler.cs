#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.TaskHandlers;

public class NewUserCreatedTaskHandler : TaskHandler<NewUserCreatedTask>
{
    public override async Task Handle(HandlerContext<NewUserCreatedTask> context)
    {
        string recipient = context.Payload.Email;
        const string Subject = "Välkommen till Snittlistan!";
        string activationKey = context.Payload.ActivationKey;
        string id = context.Payload.UserId;

        UserRegisteredEmail email = new(
            recipient,
            Subject,
            id,
            activationKey);
        await CompositionRoot.EmailService.SendAsync(email);
    }
}
