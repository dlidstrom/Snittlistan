#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.TaskHandlers;

public class OneTimeKeyTaskHandler : TaskHandler<OneTimeKeyTask>
{
    public override async Task Handle(HandlerContext<OneTimeKeyTask> context)
    {
        OneTimePasswordEmail email = new(
            context.Payload.Email,
            context.Payload.Subject,
            context.Payload.OneTimePassword);
        await CompositionRoot.EmailService.SendAsync(email);
    }
}
