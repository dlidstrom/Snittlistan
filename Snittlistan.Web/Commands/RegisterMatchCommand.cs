#nullable enable

using Raven.Client.Linq;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Areas.V2.Indexes;

namespace Snittlistan.Web.Commands;

public class RegisterMatchCommandHandler : CommandHandler<RegisterMatchCommandHandler.Command>
{
    public override Task Handle(CommandContext<Command> context)
    {
        Roster roster = CompositionRoot.DocumentSession.Load<Roster>(context.Command.RosterId);
        MatchResult matchResult = new(
            roster,
            context.Command.Result.TeamScore,
            context.Command.Result.OpponentScore,
            roster.BitsMatchId);
        Player[] players = CompositionRoot.DocumentSession.Load<Player>(roster.Players);

        MatchSerie[] matchSeries = context.Command.Result.CreateMatchSeries();

        Dictionary<string, ResultForPlayerIndex.Result> resultsForPlayer =
            CompositionRoot.DocumentSession.Query<ResultForPlayerIndex.Result, ResultForPlayerIndex>()
                .Where(x => x.Season == roster.Season)
                .ToArray()
                .ToDictionary(x => x.PlayerId);
        matchResult.RegisterSeries(
            task => context.PublishMessage(task),
            matchSeries,
            context.Command.Result.OpponentSeries,
            players,
            resultsForPlayer);
        CompositionRoot.EventStoreSession.Store(matchResult);

        return Task.CompletedTask;
    }

    public class Command : CommandBase
    {
        public Command(string rosterId, ParseResult result)
        {
            RosterId = rosterId;
            Result = result;
        }

        public string RosterId { get; }

        public ParseResult Result { get; }
    }
}
