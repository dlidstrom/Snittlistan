#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Infrastructure;

namespace Snittlistan.Web.Commands;

public class SendEmailTaskCommandHandler : CommandHandler<SendEmailTaskCommandHandler.Command>
{
    public override Task Handle(HandlerContext<Command> context)
    {
        SendEmailTask task = SendEmailTask.Create(
            context.Payload.Recipient,
            context.Payload.ReplyTo,
            context.Payload.Subject,
            context.Payload.Content,
            context.Payload.Rate,
            context.Payload.PerSeconds,
            context.Payload.Key);
        context.PublishMessage(task);
        return Task.CompletedTask;
    }

    public record Command(
        string Recipient,
        string ReplyTo,
        string Subject,
        string Content,
        int Rate,
        int PerSeconds,
        string Key);
}
