#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Database;
using Snittlistan.Web.Models;
using System.Data.Entity;

namespace Snittlistan.Web.Commands;

public class PublishRosterMailsCommandHandler : CommandHandler<PublishRosterMailsCommandHandler.Command>
{
    public override async Task Handle(HandlerContext<Command> context)
    {
        // TODO might be comma-separated values
        Roster roster = CompositionRoot.DocumentSession.LoadEx<Roster>(context.Payload.RosterKey);
        AuditLogEntry auditLogEntry =
            roster.AuditLogEntries.Single(x => x.CorrelationId == context.CorrelationId);
        RosterState before = (RosterState)auditLogEntry.Before;
        RosterState after = (RosterState)auditLogEntry.After;
        IEnumerable<string> affectedPlayers = before.Players.Concat(after.Players);

        // find user who did the last edit-players action
        AuditLogEntry? editPlayersAction = roster.AuditLogEntries.LastOrDefault(
            x => x.Action == Roster.ChangeType.EditPlayers.ToString());
        if (editPlayersAction == null)
        {
            throw new Exception($"No edit-players action found in roster {roster.Id}");
        }

        Player? editPlayer = CompositionRoot.DocumentSession.Load<Player>(editPlayersAction.UserId);
        User? editUser = CompositionRoot.DocumentSession.FindUserByEmail(editPlayersAction.UserId);
        string replyToEmail =
            editPlayer?.Email
            ?? editUser?.Email
            ?? throw new Exception($"Unable to find edit-players action user with id '{editPlayersAction.UserId}'");
        foreach (string playerId in new HashSet<string>(affectedPlayers))
        {
            PublishRosterMailTask message = new(
                context.Payload.RosterKey,
                playerId,
                replyToEmail);
            context.PublishMessage(message);
        }

        RosterMail rosterMail =
            await CompositionRoot.Databases.Snittlistan.RosterMails.SingleAsync(
                x => x.RosterKey == context.Payload.RosterKey && x.PublishedDate == null);
        rosterMail.MarkPublished(DateTime.Now);
    }

    public record Command(string RosterKey);
}
