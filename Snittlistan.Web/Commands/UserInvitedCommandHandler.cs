#nullable enable

using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.Commands;

public class UserInvitedCommandHandler : CommandHandler<UserInvitedCommandHandler.Command>
{
    public override async Task Handle(HandlerContext<Command> context)
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

    public record Command(string ActivationUri, string Email);
}
