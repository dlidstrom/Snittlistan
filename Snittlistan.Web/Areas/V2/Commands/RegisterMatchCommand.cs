namespace Snittlistan.Web.Areas.V2.Commands
{
    using System;
    using System.Linq;
    using EventStoreLite;
    using Raven.Client;
    using Raven.Client.Linq;
    using Snittlistan.Web.Areas.V2.Domain;
    using Snittlistan.Web.Areas.V2.Domain.Match;
    using Snittlistan.Web.Areas.V2.Indexes;
    using Snittlistan.Web.Infrastructure;

    public class RegisterMatchCommand : ICommand
    {
        private readonly Roster roster;
        private readonly ParseResult result;

        public RegisterMatchCommand(Roster roster, ParseResult result)
        {
            this.roster = roster ?? throw new ArgumentNullException(nameof(roster));
            this.result = result ?? throw new ArgumentNullException(nameof(result));
        }

        public void Execute(IDocumentSession session, IEventStoreSession eventStoreSession, Action<object> publish)
        {
            var matchResult = new MatchResult(
                roster,
                result.TeamScore,
                result.OpponentScore,
                roster.BitsMatchId);
            Player[] players = session.Load<Player>(roster.Players);

            MatchSerie[] matchSeries = result.CreateMatchSeries();

            var resultsForPlayer = session.Query<ResultForPlayerIndex.Result, ResultForPlayerIndex>()
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
        }
    }
}