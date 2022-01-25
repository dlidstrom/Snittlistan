#nullable enable

using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Models;
using System.Text;

namespace Snittlistan.Web.Commands;

public class SendEmailCommandHandler : CommandHandler<SendEmailCommandHandler.Command>
{
    public override async Task Handle(HandlerContext<Command> context)
    {
        SendEmail email = SendEmail.ToRecipient(
            context.Payload.To,
            Encoding.UTF8.GetString(Convert.FromBase64String(context.Payload.Subject)),
            Encoding.UTF8.GetString(Convert.FromBase64String(context.Payload.Content)));
        await CompositionRoot.EmailService.SendAsync(email);
    }

    public record Command(string To, string Subject, string Content);
}
