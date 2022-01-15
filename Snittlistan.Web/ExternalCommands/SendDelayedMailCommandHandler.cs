#nullable enable

using Snittlistan.Queue.ExternalCommands;
using Snittlistan.Queue.Messages;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Database;
using System.Data.Entity;

namespace Snittlistan.Web.ExternalCommands;

public class SendDelayedMailCommandHandler : ICommandHandler<SendDelayedMailCommand>
{
    public Databases Databases { get; set; } = null!;

    public async Task Handle(SendDelayedMailCommand command)
    {
        Tenant tenant = await Databases.Snittlistan.Tenants.SingleAsync(x => x.Hostname == command.Hostname);
        TaskPublisher taskPublisher = new(
            tenant,
            Databases,
            command.CorrelationId,
            null);
        EmailTask emailTask = EmailTask.Create(command.Recipient, command.Subject, command.Content);
        taskPublisher.PublishDelayedTask(
            emailTask,
            DateTime.Now.AddSeconds(command.DelayInSeconds),
            "system");
    }
}
