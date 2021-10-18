#nullable enable

namespace Snittlistan.Web.Areas.V2.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Snittlistan.Web.Areas.V2.Domain.Match;
    using Snittlistan.Web.Areas.V2.ReadModels;

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
            List<MatchSerie4> matchSeries = new();
            ResultSeries4ReadModel.Serie[] series = new[]
            {
                Series.ElementAtOrDefault(0),
                Series.ElementAtOrDefault(1),
                Series.ElementAtOrDefault(2),
                Series.ElementAtOrDefault(3)
            };
            int serieNumber = 1;
            foreach (ResultSeries4ReadModel.Serie serie in series.Where(x => x != null))
            {
                List<MatchGame4> games = new();
                for (int i = 0; i < 4; i++)
                {
                    ResultSeries4ReadModel.Game game = serie.Games[i];
                    MatchGame4 matchGame = new(game.Player, game.Score, game.Pins);
                    games.Add(matchGame);
                }

                matchSeries.Add(new MatchSerie4(serieNumber++, games));
            }

            return matchSeries.ToArray();
        }

        public List<string> GetPlayerIds()
        {
            IEnumerable<string> query = from game in Series.First().Games
                                        select game.Player;
            string[] playerIds = query.ToArray();
            HashSet<string> playerIdsWithoutReserve = new(playerIds);
            IEnumerable<string> restQuery = from serie in Series
                                            from game in serie.Games
                                            where playerIdsWithoutReserve.Contains(game.Player) == false
                                            select game.Player;
            List<string> allPlayerIds = playerIds.Concat(
                new HashSet<string>(restQuery).Where(x => playerIdsWithoutReserve.Contains(x) == false)).ToList();
            return allPlayerIds;
        }

    }
}
