using System;
using System.Collections.Generic;
using System.Linq;
using EventStoreLite;
using Raven.Client;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Infrastructure;

namespace Snittlistan.Web.Areas.V2.Controllers.Api
{
    public class RegisterMatchCommand : ICommand
    {
        private readonly Roster roster;
        private readonly ParseResult result;

        public RegisterMatchCommand(Roster roster, ParseResult result)
        {
            if (roster == null) throw new ArgumentNullException("roster");
            if (result == null) throw new ArgumentNullException("result");
            this.roster = roster;
            this.result = result;
        }

        public void Execute(IDocumentSession session, IEventStoreSession eventStoreSession)
        {
            var matchResult = new MatchResult(
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
                var tables = new List<MatchTable>();
                for (var i = 0; i < 4; i++)
                {
                    var game1 = new MatchGame(
                        serie.Tables[i].Game1.Player,
                        serie.Tables[i].Game1.Pins,
                        serie.Tables[i].Game1.Strikes,
                        serie.Tables[i].Game1.Spares);
                    var game2 = new MatchGame(
                        serie.Tables[i].Game2.Player,
                        serie.Tables[i].Game2.Pins,
                        serie.Tables[i].Game2.Strikes,
                        serie.Tables[i].Game2.Spares);
                    tables.Add(new MatchTable(game1, game2, serie.Tables[i].Score));
                }

                matchResult.RegisterSerie(new MatchSerie(tables));
            }

            eventStoreSession.Store(matchResult);
        }
    }
}