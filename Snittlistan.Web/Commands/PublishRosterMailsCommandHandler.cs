#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Database;
using System.Data.Entity;

namespace Snittlistan.Web.Commands;

public class PublishRosterMailsCommandHandler : CommandHandler<PublishRosterMailsCommandHandler.Command>
{
    public override async Task Handle(HandlerContext<Command> context)
    {
        // TODO might be comma-separated values
        Roster roster = CompositionRoot.DocumentSession.Load<Roster>(context.Payload.RosterKey);
        AuditLogEntry auditLogEntry = roster.AuditLogEntries.Single(x => x.CorrelationId == context.CorrelationId);
        RosterState before = (RosterState)auditLogEntry.Before;
        RosterState after = (RosterState)auditLogEntry.After;
        IEnumerable<string> affectedPlayers = before.Players.Concat(after.Players);
        foreach (string playerId in new HashSet<string>(affectedPlayers))
        {
            PublishRosterMailTask message = new(
                context.Payload.RosterKey,
                playerId);
            context.PublishMessage(message);
        }

        RosterMail rosterMail =
            await CompositionRoot.Databases.Snittlistan.RosterMails.SingleAsync(
                x => x.RosterKey == context.Payload.RosterKey && x.PublishedDate == null);
        rosterMail.MarkPublished(DateTime.Now);
    }

    public record Command(string RosterKey);
}
