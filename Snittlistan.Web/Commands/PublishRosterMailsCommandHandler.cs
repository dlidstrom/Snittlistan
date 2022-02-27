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
        HashSet<string> affectedPlayers = new(before.Players.Concat(after.Players));

        // find user who did the last edit-players action
        AuditLogEntry? editPlayersAction = roster.AuditLogEntries.LastOrDefault(
            x => x.Action == Roster.ChangeType.EditPlayers.ToString());
        if (editPlayersAction == null)
        {
            throw new Exception($"No edit-players action found in roster {roster.Id}");
        }

        Dictionary<string, Player> playersDict =
            CompositionRoot.DocumentSession.Load<Player>(affectedPlayers)
            .ToDictionary(x => x.Id);
        Player? editPlayer = CompositionRoot.DocumentSession.Load<Player>(editPlayersAction.UserId);
        User? editUser = CompositionRoot.DocumentSession.FindUserByEmail(editPlayersAction.UserId);
        string replyToEmail =
            editPlayer?.Email
            ?? editUser?.Email
            ?? throw new Exception($"Unable to find edit-players action user with id '{editPlayersAction.UserId}'");
        if (editPlayer is not null)
        {
            _ = affectedPlayers.Add(editPlayer.Id);
            playersDict[editPlayer.Id] = editPlayer;
        }

        foreach (string playerId in affectedPlayers)
        {
            Player player = playersDict[playerId];
            if (string.IsNullOrEmpty(player.Email) == false)
            {
                PublishRosterMailTask message = new(
                    context.Payload.RosterKey,
                    player.Email,
                    player.Nickname ?? player.Name,
                    replyToEmail,
                    context.Payload.RosterLink);
                await context.PublishMessage(message);
            }
        }

        RosterMail rosterMail =
            await CompositionRoot.Databases.Snittlistan.RosterMails.SingleAsync(
                x => x.RosterKey == context.Payload.RosterKey && x.PublishedDate == null);
        rosterMail.MarkPublished(DateTime.Now);
    }

    public record Command(
        string RosterKey,
        Uri RosterLink);
}
