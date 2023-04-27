#nullable enable

using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Database;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.Commands;

public class PublishRosterMailCommandHandler
    : HandleMailCommandHandler<PublishRosterMailCommandHandler.Command, UpdateRosterEmail>
{
    protected override async Task<UpdateRosterEmail> CreateEmail(HandlerContext<Command> context)
    {
        Roster roster = CompositionRoot.DocumentSession
            .Include<Roster>(x => x.Players)
            .Load<Roster>(context.Payload.RosterId);
        FormattedAuditLog formattedAuditLog = roster.GetFormattedAuditLog(
            CompositionRoot.DocumentSession,
            context.CorrelationId);
        Player[] players = CompositionRoot.DocumentSession.Load<Player>(roster.Players);
        string teamLeader =
            roster.TeamLeader != null
            ? CompositionRoot.DocumentSession.Load<Player>(roster.TeamLeader).Name
            : string.Empty;
        bool needsAccept = roster.AcceptedPlayers.Contains(context.Payload.PlayerId) == false;
        UpdateRosterEmail_State.MatchHeadType matchHead;
        if (roster.BitsMatchId != 0)
        {
            Bits_VMatchHeadInfo? matchHeadInfo = await context.Databases.Bits.VMatchHeadInfo.SingleOrDefaultAsync(
                x => x.ExternalMatchId == roster.BitsMatchId);
            if (matchHeadInfo is not null)
            {
                matchHead = new(
                    "Hemmalag",
                    matchHeadInfo.HomeTeamAlias,
                    "Bortalag",
                    matchHeadInfo.AwayTeamAlias,
                    matchHeadInfo.HallName,
                    matchHeadInfo.OilProfileId,
                    matchHeadInfo.OilProfileName,
                    matchHeadInfo.MatchDateTime);
            }
            else
            {
                int? oilPatternId = null;
                if (roster.OilPattern is not null)
                {
                    int.TryParse(
                        roster.OilPattern.Url.Substring(roster.OilPattern.Url.LastIndexOf('/') + 1),
                        out oilPatternId);
                }

                matchHead = new(
                    "Lag",
                    roster.Team,
                    "Motståndare",
                    roster.Opponent,
                    roster.Location,
                    oilPatternId ?? 0,
                    roster.OilPattern?.Name ?? "Ingen oljeprofil",
                    roster.Date);
            }
        }
        else
        {
            matchHead = new(
                "Lag",
                roster.Team,
                "Motståndare",
                roster.Opponent ?? string.Empty,
                roster.Location ?? string.Empty,
                null,
                roster.OilPattern.Name,
                roster.Date);
        }

        UpdateRosterEmail email = new(
            context.Payload.RecipientEmail,
            context.Payload.RecipientName,
            formattedAuditLog,
            players.Select(x => x.Name).ToArray(),
            teamLeader,
            context.Payload.ReplyToEmail,
            roster.Season,
            roster.Turn,
            context.Payload.RosterLink,
            context.Payload.UserProfileLink,
            needsAccept,
            matchHead);
        return email;
    }

    protected override RatePerSeconds GetRate(HandlerContext<Command> context)
    {
        RatePerSeconds ratePerSeconds = new(
            $"roster_mail:{context.Payload.RecipientEmail}:{context.Tenant.TenantId}",
            1,
            600);
        return ratePerSeconds;
    }

    public record Command(
        string RosterId,
        string PlayerId,
        string RecipientEmail,
        string RecipientName,
        string ReplyToEmail,
        Uri RosterLink,
        Uri UserProfileLink);
}
