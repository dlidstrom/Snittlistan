#nullable enable

using Snittlistan.Model;
using Snittlistan.Queue.Messages;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Database;
using Snittlistan.Web.Models;

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

        // only send to players that have accepted email, or have not yet decided
        string[] propertyKeys = affectedPlayers.Select(UserSettings.GetKey).ToArray();
        Dictionary<string, UserSettings> properties =
            Enumerable.ToDictionary(
                await CompositionRoot.Databases.Snittlistan.KeyValueProperties.Where(
                    x => propertyKeys.Contains(x.Key))
                .ToArrayAsync(),
                x => x.Key,
                x => (UserSettings)x.Value);

        foreach (string playerId in affectedPlayers)
        {
            Player player = playersDict[playerId];
            do
            {
                if (properties.TryGetValue(
                    UserSettings.GetKey(playerId),
                    out UserSettings? settings))
                {
                    Logger.InfoFormat(
                        "found user settings for {playerId} {@settings}",
                        playerId,
                        settings);
                }
                else
                {
                    Logger.InfoFormat(
                        "no user settings found for {playerId}",
                        playerId);
                }

                if (string.IsNullOrEmpty(player.Email))
                {
                    Logger.InfoFormat("no email set for player {playerId}", playerId);
                    break;
                }

                if ((settings?.RosterMailEnabled ?? true) == false)
                {
                    Logger.InfoFormat("roster mail disabled for player {playerId}");
                    break;
                }

                PublishRosterMailTask message = new(
                    context.Payload.RosterKey,
                    player.Email,
                    player.Nickname ?? player.Name,
                    replyToEmail,
                    context.Payload.RosterLink,
                    context.Payload.UserProfileLink);
                context.PublishMessage(message);
            }
            while (false);
        }

        RosterMail rosterMail =
            await CompositionRoot.Databases.Snittlistan.RosterMails.SingleAsync(
                x => x.RosterKey == context.Payload.RosterKey && x.PublishedDate == null);
        rosterMail.MarkPublished(DateTime.Now);
    }

    public record Command(
        string RosterKey,
        Uri RosterLink,
        Uri UserProfileLink);
}
