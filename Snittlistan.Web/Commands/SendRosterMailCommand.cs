#nullable enable

using Snittlistan.Queue.Messages;

namespace Snittlistan.Web.Commands;

public class CreateRosterMailCommandHandler : CommandHandler<CreateRosterMailCommandHandler.Command>
{
    public override Task Handle(CommandContext<Command> context)
    {
        IQueryable<string> query =
            from rosterMail in CompositionRoot.Databases.Snittlistan.RosterMails
            where rosterMail.RosterId == context.Command.RosterId
            select rosterMail.RosterId;
        if (query.Any() == false)
        {
            _ = CompositionRoot.Databases.Snittlistan.RosterMails.Add(new(context.Command.RosterId));
            InitiateUpdateMailTask task = new(context.Command.RosterId, 0, context.CorrelationId);
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
