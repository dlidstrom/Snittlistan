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

    public MsmqFactory MsmqFactory { get; set; } = null!;

    public async Task Handle(SendDelayedMailCommand command)
    {
        Tenant tenant = await Databases.Snittlistan.Tenants.SingleAsync(x => x.Hostname == command.Hostname);
        TaskPublisher taskPublisher = new(
            tenant,
            Databases,
            MsmqFactory,
            command.CorrelationId,
            null);
        SendEmailTask emailTask = SendEmailTask.Create(
            command.Recipient,
            command.ReplyTo,
            command.Subject,
            command.Content,
            command.RatePerSeconds,
            DateTime.Now.Ticks.ToString());
        taskPublisher.PublishDelayedTask(
            emailTask,
            DateTime.Now.AddSeconds(command.DelayInSeconds),
            "system");
    }
}
