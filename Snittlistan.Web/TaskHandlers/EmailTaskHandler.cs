#nullable enable

using System.Text;
using Snittlistan.Queue.Messages;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.TaskHandlers;

public class EmailTaskHandler : TaskHandler<EmailTask>
{
    public override async Task Handle(HandlerContext<EmailTask> context)
    {
        SendEmail email = SendEmail.ToRecipient(
            context.Payload.To,
            Encoding.UTF8.GetString(Convert.FromBase64String(context.Payload.Subject)),
            Encoding.UTF8.GetString(Convert.FromBase64String(context.Payload.Content)));
        await CompositionRoot.EmailService.SendAsync(email);
    }
}
