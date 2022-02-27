#nullable enable

using Raven.Client.Linq;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Web.Infrastructure;

namespace Snittlistan.Web.Commands;

public class RegisterMatchCommandHandler : CommandHandler<RegisterMatchCommandHandler.Command>
{
    public override async Task Handle(HandlerContext<Command> context)
    {
        Roster roster = CompositionRoot.DocumentSession.Load<Roster>(context.Payload.RosterId);
        MatchResult matchResult = new(
            roster,
            context.Payload.Result.TeamScore,
            context.Payload.Result.OpponentScore,
            roster.BitsMatchId);
        Player[] players = CompositionRoot.DocumentSession.Load<Player>(roster.Players);

        MatchSerie[] matchSeries = context.Payload.Result.CreateMatchSeries();

        Dictionary<string, ResultForPlayerIndex.Result> resultsForPlayer =
            CompositionRoot.DocumentSession.Query<ResultForPlayerIndex.Result, ResultForPlayerIndex>()
                .Where(x => x.Season == roster.Season)
                .ToArray()
                .ToDictionary(x => x.PlayerId);
        await matchResult.RegisterSeries(
            async task => await context.PublishMessage(task),
            matchSeries,
            context.Payload.Result.OpponentSeries,
            players,
            resultsForPlayer);
        CompositionRoot.EventStoreSession.Store(matchResult);
    }

    public record Command(
        string RosterId,
        ParseResult Result);
}
