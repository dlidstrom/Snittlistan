#nullable enable

using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Infrastructure;

namespace Snittlistan.Web.Commands;

public class RegisterMatchManualCommandHandler : CommandHandler<RegisterMatchManualCommandHandler.Command>
{
    public override Task Handle(HandlerContext<Command> context)
    {
        Roster roster = CompositionRoot.DocumentSession.Load<Roster>(context.Payload.RosterId);
        MatchResult matchResult = new(
            roster,
            context.Payload.TeamScore,
            context.Payload.OpponentScore,
            roster.BitsMatchId);
        Player[] players = CompositionRoot.DocumentSession.Load<Player>(roster.Players);

        matchResult.RegisterSeries(
            task => context.PublishMessage(task),
            context.Payload.MatchSeries,
            players,
            context.Payload.SummaryText,
            context.Payload.SummaryHtml);
        CompositionRoot.EventStoreSession.Store(matchResult);

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
