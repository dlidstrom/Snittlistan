#nullable enable

using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.Commands;

public class PublishRosterMailCommandHandler
    : HandleMailCommandHandler<PublishRosterMailCommandHandler.Command, UpdateRosterEmail>
{
    protected override Task<UpdateRosterEmail> CreateEmail(HandlerContext<Command> context)
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
            context.Payload.UserProfileLink);
        return Task.FromResult(email);
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
        string RecipientEmail,
        string RecipientName,
        string ReplyToEmail,
        Uri RosterLink,
        Uri UserProfileLink);
}
