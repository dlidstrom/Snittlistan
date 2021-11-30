using Snittlistan.Queue.Messages;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Models;

#nullable enable

namespace Snittlistan.Web.Areas.V2.Tasks;
public class OneTimeKeyTaskHandler : TaskHandler<OneTimeKeyTask>
{
    public override async Task Handle(MessageContext<OneTimeKeyTask> context)
    {
        OneTimePasswordEmail email = new(
            context.Task.Email,
            context.Task.Subject,
            context.Task.OneTimePassword);
        await EmailService.SendAsync(email);
    }
}
