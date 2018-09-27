using System;
using System.Collections.Generic;
using System.Linq;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.Domain
{
    public class Parse4Result
    {
        public Parse4Result(
            int teamScore,
            int awayScore,
            int turn,
            ResultSeries4ReadModel.Serie[] series)
        {
            Series = series ?? throw new ArgumentNullException(nameof(series));
            OpponentScore = awayScore;
            Turn = turn;
            TeamScore = teamScore;
        }

        public int TeamScore { get; }

        public int OpponentScore { get; }

        public int Turn { get; }

        public ResultSeries4ReadModel.Serie[] Series { get; }

        public MatchSerie4[] CreateMatchSeries()
        {
            var matchSeries = new List<MatchSerie4>();
            var series = new[]
            {
                Series.ElementAtOrDefault(0),
                Series.ElementAtOrDefault(1),
                Series.ElementAtOrDefault(2),
                Series.ElementAtOrDefault(3)
            };
            var serieNumber = 1;
            foreach (var serie in series.Where(x => x != null))
            {
                var games = new List<MatchGame4>();
                for (var i = 0; i < 4; i++)
                {
                    var game = serie.Games[i];
                    var matchGame = new MatchGame4(game.Player, game.Score, game.Pins);
                    games.Add(matchGame);
                }

                matchSeries.Add(new MatchSerie4(serieNumber++, games));
            }

            return matchSeries.ToArray();
        }
    }
}