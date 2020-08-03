namespace Snittlistan.Web.Areas.V2.Commands
{
    using System;
    using EventStoreLite;
    using Raven.Client;
    using Snittlistan.Web.Areas.V2.Domain;
    using Snittlistan.Web.Areas.V2.Domain.Match;
    using Snittlistan.Web.Infrastructure;

    public class RegisterMatch4Command : ICommand
    {
        private readonly Roster roster;
        private readonly Parse4Result result;
        private readonly string summaryText;
        private readonly string summaryHtml;

        public RegisterMatch4Command(
            Roster roster,
            Parse4Result result,
            string summaryText = null,
            string summaryHtml = null)
        {
            this.roster = roster ?? throw new ArgumentNullException(nameof(roster));
            this.result = result ?? throw new ArgumentNullException(nameof(result));
            this.summaryText = summaryText;
            this.summaryHtml = summaryHtml;
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
            Player[] players = session.Load<Player>(roster.Players);

            MatchSerie4[] matchSeries = result.CreateMatchSeries();
            matchResult.RegisterSeries(
                publish,
                matchSeries,
                players,
                summaryText ?? string.Empty,
                summaryHtml ?? string.Empty);
            eventStoreSession.Store(matchResult);
        }
    }
}