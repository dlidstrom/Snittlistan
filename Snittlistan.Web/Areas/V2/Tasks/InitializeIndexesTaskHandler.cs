#nullable enable

namespace Snittlistan.Web.Areas.V2.Tasks
{
    using System.Threading.Tasks;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Web.Infrastructure.Indexes;
    using Snittlistan.Web.Models;

    public class InitializeIndexesTaskHandler : TaskHandler<InitializeIndexesTask>
    {
        public override async Task Handle(MessageContext<InitializeIndexesTask> task)
        {
            IndexCreator.CreateIndexes(DocumentStore);
            User admin = DocumentSession.Load<User>(User.AdminId);
            if (admin == null)
            {
                admin = new("", "", task.Task.Email, task.Task.Password)
                {
                    Id = User.AdminId
                };
                await admin.Initialize(async t => await TaskPublisher.PublishTask(t, "system"));
                admin.Activate();
                DocumentSession.Store(admin);
            }
            else
            {
                admin.SetEmail(task.Task.Email);
                admin.SetPassword(task.Task.Password);
                admin.Activate();
            }
        }
    }
}
