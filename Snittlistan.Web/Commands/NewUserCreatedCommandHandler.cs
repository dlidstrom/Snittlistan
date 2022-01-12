#nullable enable

using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.Commands;

public class NewUserCreatedCommandHandler : CommandHandler<NewUserCreatedCommandHandler.Command>
{
    public override async Task Handle(HandlerContext<Command> context)
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

    public record Command(string Email, string ActivationKey, string UserId);
}
