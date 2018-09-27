using System;
using System.Collections.Generic;
using System.Linq;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.Domain
{
    public class ParseResult
    {
        public ParseResult(
            int teamScore,
            int opponentScore,
            int turn,
            ResultSeriesReadModel.Serie[] series,
            ResultSeriesReadModel.Serie[] opponentSeries)
        {
            TeamScore = teamScore;
            OpponentScore = opponentScore;
            Turn = turn;
            Series = series ?? throw new ArgumentNullException(nameof(series));
            OpponentSeries = opponentSeries ?? throw new ArgumentNullException(nameof(opponentSeries));
        }

        public int TeamScore { get; }

        public int OpponentScore { get; }

        public int Turn { get; }

        public ResultSeriesReadModel.Serie[] Series { get; }

        public ResultSeriesReadModel.Serie[] OpponentSeries { get; }

        public MatchSerie[] CreateMatchSeries()
        {
            var matchSeries = new List<MatchSerie>();
            var serieNumber = 1;
            var series = new[]
            {
                Series.ElementAtOrDefault(0),
                Series.ElementAtOrDefault(1),
                Series.ElementAtOrDefault(2),
                Series.ElementAtOrDefault(3)
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
                    tables.Add(new MatchTable(i + 1, game1, game2, serie.Tables[i].Score));
                }

                matchSeries.Add(new MatchSerie(serieNumber++, tables));
            }

            return matchSeries.ToArray();
        }
    }
}