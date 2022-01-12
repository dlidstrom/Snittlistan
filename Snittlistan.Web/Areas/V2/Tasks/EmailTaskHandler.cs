#nullable enable

using System.Text;
using Snittlistan.Queue.Messages;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.Areas.V2.Tasks;

public class EmailTaskHandler : TaskHandler<EmailTask>
{
    public override async Task Handle(MessageContext<EmailTask> context)
    {
        SendEmail email = SendEmail.ToRecipient(
            context.Task.To,
            Encoding.UTF8.GetString(Convert.FromBase64String(context.Task.Subject)),
            Encoding.UTF8.GetString(Convert.FromBase64String(context.Task.Content)));
        await CompositionRoot.EmailService.SendAsync(email);
    }
}
