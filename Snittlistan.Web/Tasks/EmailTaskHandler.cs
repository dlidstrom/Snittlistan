using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.BackgroundTasks;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.Tasks
{
    public class EmailTaskHandler : IBackgroundTaskHandler<EmailTask>
    {
        public void Handle(EmailTask task, TenantConfiguration tenantConfiguration)
        {
            Emails.SendMail(task.Recipient, task.Subject, task.Content);
        }
    }
}