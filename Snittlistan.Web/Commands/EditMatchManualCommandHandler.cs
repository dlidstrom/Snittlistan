#nullable enable

using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Infrastructure;

namespace Snittlistan.Web.Commands;

public class EditMatchManualCommandHandler : CommandHandler<EditMatchManualCommandHandler.Command>
{
    public override Task Handle(HandlerContext<Command> context)
    {
        Roster roster = CompositionRoot.DocumentSession.Load<Roster>(context.Payload.RosterId);
        MatchResult? matchResult = CompositionRoot.EventStoreSession.Load<MatchResult>(roster.MatchResultId!);
        Player[] players = CompositionRoot.DocumentSession.Load<Player>(roster.Players);

        matchResult!.UpdateManual(
            task => context.PublishMessage(task),
            roster,
            context.Payload.TeamScore,
            context.Payload.OpponentScore,
            context.Payload.MatchSeries,
            players,
            context.Payload.SummaryText,
            context.Payload.SummaryHtml);

        return Task.CompletedTask;
    }

    public record Command(
        string RosterId,
        int TeamScore,
        int OpponentScore,
        MatchSerie[] MatchSeries,
        string SummaryText,
        string SummaryHtml);
}
