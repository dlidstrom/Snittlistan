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

    protected override string GetKey(Command command)
    {
        return $"send-email/{command.RatePerSeconds}:{command.To}";
    }

    protected override RatePerSeconds GetRate(Command command)
    {
        return new(1, command.RatePerSeconds);
    }

    public record Command(string To, string Subject, string Content, int RatePerSeconds);
}
