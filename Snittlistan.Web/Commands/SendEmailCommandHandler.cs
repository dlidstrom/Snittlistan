#nullable enable

using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Models;
using System.Text;

namespace Snittlistan.Web.Commands;

public class SendEmailCommandHandler
    : HandleMailCommandHandler<SendEmailCommandHandler.Command, SendEmail>
{
    protected override Task<SendEmail> CreateEmail(HandlerContext<Command> context)
    {
        SendEmail email = SendEmail.ToRecipient(
            context.Payload.To,
            Encoding.UTF8.GetString(Convert.FromBase64String(context.Payload.Subject)),
            Encoding.UTF8.GetString(Convert.FromBase64String(context.Payload.Content)));
        return Task.FromResult(email);
    }

    protected override RatePerSeconds GetRate(HandlerContext<Command> context)
    {
        return new(
            $"send-email/{context.Payload.RatePerSeconds}:{context.Payload.To}",
            1,
            context.Payload.RatePerSeconds);
    }

    public record Command(string To, string Subject, string Content, int RatePerSeconds);
}
