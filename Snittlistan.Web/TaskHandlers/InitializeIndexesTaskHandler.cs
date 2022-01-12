#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Indexes;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.TaskHandlers;

public class InitializeIndexesTaskHandler : TaskHandler<InitializeIndexesTask>
{
    public override Task Handle(MessageContext<InitializeIndexesTask> context)
    {
        IndexCreator.CreateIndexes(CompositionRoot.DocumentStore);
        User admin = CompositionRoot.DocumentSession.Load<User>(User.AdminId);
        if (admin == null)
        {
            admin = new("", "", context.Task.Email, context.Task.Password)
            {
                Id = User.AdminId
            };
            admin.Initialize(t => context.PublishMessage(t));
            admin.Activate();
            CompositionRoot.DocumentSession.Store(admin);
        }
        else
        {
            admin.SetEmail(context.Task.Email);
            admin.SetPassword(context.Task.Password);
            admin.Activate();
        }

        return Task.CompletedTask;
    }
}
