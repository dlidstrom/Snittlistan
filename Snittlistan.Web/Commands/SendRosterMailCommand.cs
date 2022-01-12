#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Infrastructure;

namespace Snittlistan.Web.Commands;

public class CreateRosterMailCommandHandler : CommandHandler<CreateRosterMailCommandHandler.Command>
{
    public override Task Handle(HandlerContext<Command> context)
    {
        IQueryable<string> query =
            from rosterMail in CompositionRoot.Databases.Snittlistan.RosterMails
            where rosterMail.RosterId == context.Payload.RosterId
            select rosterMail.RosterId;
        if (query.Any() == false)
        {
            _ = CompositionRoot.Databases.Snittlistan.RosterMails.Add(new(context.Payload.RosterId));
            InitiateUpdateMailTask task = new(context.Payload.RosterId, 0, context.CorrelationId);
            context.PublishMessage(task, DateTime.Now.AddMinutes(10));
        }

        return Task.CompletedTask;
    }

    public class Command : CommandBase
    {
        public string RosterId { get; }

        public Command(string rosterId)
        {
            RosterId = rosterId;
        }
    }
}
