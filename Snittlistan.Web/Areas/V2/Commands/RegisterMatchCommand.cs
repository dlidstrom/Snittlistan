using EventStoreLite;
using Raven.Client;
using Raven.Client.Linq;
using Snittlistan.Queue.Messages;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Web.Infrastructure;

#nullable enable

namespace Snittlistan.Web.Areas.V2.Commands;
public class RegisterMatchCommand : ICommand
{
    private readonly Roster roster;
    private readonly ParseResult result;

    public RegisterMatchCommand(Roster roster, ParseResult result)
    {
        this.roster = roster ?? throw new ArgumentNullException(nameof(roster));
        this.result = result ?? throw new ArgumentNullException(nameof(result));
    }

    public Task Execute(
        IDocumentSession session,
        IEventStoreSession eventStoreSession,
        Action<TaskBase> publish)
    {
        MatchResult matchResult = new(
            roster,
            result.TeamScore,
            result.OpponentScore,
            roster.BitsMatchId);
        Player[] players = session.Load<Player>(roster.Players);

        MatchSerie[] matchSeries = result.CreateMatchSeries();

        Dictionary<string, ResultForPlayerIndex.Result> resultsForPlayer = session.Query<ResultForPlayerIndex.Result, ResultForPlayerIndex>()
            .Where(x => x.Season == roster.Season)
            .ToArray()
            .ToDictionary(x => x.PlayerId);
        matchResult.RegisterSeries(
            publish,
            matchSeries,
            result.OpponentSeries,
            players,
            resultsForPlayer);
        eventStoreSession.Store(matchResult);

        return Task.CompletedTask;
    }
}
