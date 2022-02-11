#nullable enable

using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Indexes;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.Commands;

public class InitializeIndexesCommandHandler : CommandHandler<InitializeIndexesCommandHandler.Command>
{
    public override Task Handle(HandlerContext<Command> context)
    {
        IndexCreator.CreateIndexes(CompositionRoot.DocumentStore);
        User admin = CompositionRoot.DocumentSession.Load<User>(User.AdminId);
        if (admin == null)
        {
            admin = new("", "", context.Payload.Email, context.Payload.Password)
            {
                Id = User.AdminId
            };
            admin.Initialize(t => context.PublishMessage(t));
            admin.Activate();
            CompositionRoot.DocumentSession.Store(admin);
        }
        else
        {
            admin.SetEmail(context.Payload.Email);
            admin.SetPassword(context.Payload.Password);
            admin.Activate();
        }

        return Task.CompletedTask;
    }

    public record Command(string Email, string Password);
}
