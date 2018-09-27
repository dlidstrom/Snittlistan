using System;
using EventStoreLite;
using Raven.Client;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Infrastructure;

namespace Snittlistan.Web.Areas.V2.Commands
{
    public class RegisterMatch4Command : ICommand
    {
        private readonly Roster roster;
        private readonly Parse4Result result;
        private readonly string summaryText;

        public RegisterMatch4Command(
            Roster roster,
            Parse4Result result,
            string summaryText = null)
        {
            this.roster = roster ?? throw new ArgumentNullException(nameof(roster));
            this.result = result ?? throw new ArgumentNullException(nameof(result));
            this.summaryText = summaryText;
        }

        public void Execute(
            IDocumentSession session,
            IEventStoreSession eventStoreSession,
            Action<object> publish)
        {
            var matchResult = new MatchResult4(
                roster,
                result.TeamScore,
                result.OpponentScore,
                roster.BitsMatchId);
            var players = session.Load<Player>(roster.Players);

            var matchSeries = result.CreateMatchSeries();
            matchResult.RegisterSeries(publish, matchSeries, players, summaryText ?? string.Empty);
            eventStoreSession.Store(matchResult);
        }
    }
}