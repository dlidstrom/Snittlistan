#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Web.Commands;

public class InitiateUpdateMailCommandHandler : CommandHandler<InitiateUpdateMailCommandHandler.Command>
{
    public override Task Handle(Infrastructure.HandlerContext<Command> context)
    {
        Roster roster = CompositionRoot.DocumentSession.Load<Roster>(context.Payload.RosterId);
        AuditLogEntry auditLogEntry = roster.AuditLogEntries.Single(x => x.CorrelationId == context.CorrelationId);
        RosterState before = (RosterState)auditLogEntry.Before;
        RosterState after = (RosterState)auditLogEntry.After;
        IEnumerable<string> affectedPlayers = before.Players.Concat(after.Players);
        foreach (string playerId in new HashSet<string>(affectedPlayers))
        {
            SendUpdateMailTask message = new(
                context.Payload.RosterId,
                playerId);
            context.PublishMessage(message);
        }

        return Task.CompletedTask;
    }

    public record Command(string RosterId);
}
