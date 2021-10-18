#nullable enable

namespace Snittlistan.Web.Areas.V2.Tasks
{
    using System.Threading.Tasks;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Web.Models;

    public class OneTimeKeyEventHandler : TaskHandler<OneTimeKeyEvent>
    {
        public override async Task Handle(OneTimeKeyEvent task)
        {
            OneTimePasswordEmail email = new(
                task.Email,
                task.Subject,
                task.OneTimePassword);
            await EmailService.SendAsync(email);
        }
    }
}
