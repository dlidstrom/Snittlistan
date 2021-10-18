#nullable enable

namespace Snittlistan.Web.Areas.V2.Tasks
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Web.Models;

    public class EmailTaskHandler : TaskHandler<EmailTask>
    {
        public override async Task Handle(EmailTask task)
        {
            SendEmail email = SendEmail.ToRecipient(
                task.To,
                Encoding.UTF8.GetString(Convert.FromBase64String(task.Subject)),
                Encoding.UTF8.GetString(Convert.FromBase64String(task.Content)));
            await EmailService.SendAsync(email);
        }
    }
}
