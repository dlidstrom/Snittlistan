#nullable enable

namespace Snittlistan.Web.Areas.V2.Tasks
{
    using System.Threading.Tasks;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Web.Infrastructure.Indexes;
    using Snittlistan.Web.Models;

    public class InitializeIndexesTaskHandler : TaskHandler<InitializeIndexesTask>
    {
        public override Task Handle(InitializeIndexesTask task)
        {
            IndexCreator.CreateIndexes(DocumentStore);
            User admin = DocumentSession.Load<User>(User.AdminId);
            if (admin == null)
            {
                admin = new("", "", task.Email, task.Password)
                {
                    Id = User.AdminId
                };
                admin.Initialize(PublishMessage);
                admin.Activate();
                DocumentSession.Store(admin);
            }
            else
            {
                admin.SetEmail(task.Email);
                admin.SetPassword(task.Password);
                admin.Activate();
            }

            return Task.CompletedTask;
        }
    }
}
