#nullable enable

using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Infrastructure;

namespace Snittlistan.Web.Commands;

public class RegisterMatch4CommandHandler : CommandHandler<RegisterMatch4CommandHandler.Command>
{
    public override async Task Handle(HandlerContext<Command> context)
    {
        Roster roster = CompositionRoot.DocumentSession.Load<Roster>(context.Payload.RosterId);
        MatchResult4 matchResult = new(
            roster,
            context.Payload.Result.TeamScore,
            context.Payload.Result.OpponentScore,
            roster.BitsMatchId);
        Player[] players = CompositionRoot.DocumentSession.Load<Player>(roster.Players);

        MatchSerie4[] matchSeries = context.Payload.Result.CreateMatchSeries();
        await matchResult.RegisterSeries(
            async task => await context.PublishMessage(task),
            matchSeries,
            players,
            context.Payload.SummaryText ?? string.Empty,
            context.Payload.SummaryHtml ?? string.Empty);
        CompositionRoot.EventStoreSession.Store(matchResult);
    }

    public record Command(
        string RosterId,
        Parse4Result Result,
        string? SummaryText,
        string? SummaryHtml);
}
