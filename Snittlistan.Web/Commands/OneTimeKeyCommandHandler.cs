#nullable enable

using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.Commands;

public class OneTimeKeyCommandHandler : CommandHandler<OneTimeKeyCommandHandler.Command>
{
    public override async Task Handle(HandlerContext<Command> context)
    {
        OneTimePasswordEmail email = new(
            context.Payload.Email,
            context.Payload.Subject,
            context.Payload.OneTimePassword);
        await CompositionRoot.EmailService.SendAsync(email);
    }

    public record Command(string Subject, string Email, string OneTimePassword);
}
