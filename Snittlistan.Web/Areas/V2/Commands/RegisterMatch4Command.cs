using System;
using System.Collections.Generic;
using System.Linq;
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

        public RegisterMatch4Command(Roster roster, Parse4Result result)
        {
            this.roster = roster ?? throw new ArgumentNullException(nameof(roster));
            this.result = result ?? throw new ArgumentNullException(nameof(result));
        }

        public void Execute(IDocumentSession session, IEventStoreSession eventStoreSession)
        {
            var matchResult = new MatchResult4(
                roster,
                result.TeamScore,
                result.OpponentScore,
                roster.BitsMatchId);
            var series = new[]
            {
                result.Series.ElementAtOrDefault(0),
                result.Series.ElementAtOrDefault(1),
                result.Series.ElementAtOrDefault(2),
                result.Series.ElementAtOrDefault(3)
            };
            foreach (var serie in series.Where(x => x != null))
            {
                var games = new List<MatchGame4>();
                for (var i = 0; i < 4; i++)
                {
                    var game = serie.Games[i];
                    var matchGame = new MatchGame4(game.Player, game.Score, game.Pins);
                    games.Add(matchGame);
                }

                matchResult.RegisterSerie(new MatchSerie4(games));
            }

            eventStoreSession.Store(matchResult);
        }
    }
}