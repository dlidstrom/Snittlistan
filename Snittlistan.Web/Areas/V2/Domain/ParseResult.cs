namespace Snittlistan.Web.Areas.V2.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Snittlistan.Web.Areas.V2.Domain.Match;
    using Snittlistan.Web.Areas.V2.ReadModels;

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
            List<MatchSerie> matchSeries = new();
            int serieNumber = 1;
            ResultSeriesReadModel.Serie[] series = new[]
            {
                Series.ElementAtOrDefault(0),
                Series.ElementAtOrDefault(1),
                Series.ElementAtOrDefault(2),
                Series.ElementAtOrDefault(3)
            };
            foreach (ResultSeriesReadModel.Serie serie in series.Where(x => x != null))
            {
                List<MatchTable> tables = new();
                for (int i = 0; i < 4; i++)
                {
                    MatchGame game1 = new(
                        serie.Tables[i].Game1.Player,
                        serie.Tables[i].Game1.Pins,
                        serie.Tables[i].Game1.Strikes,
                        serie.Tables[i].Game1.Spares);
                    MatchGame game2 = new(
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

        public List<string> GetPlayerIds()
        {
            IEnumerable<string> query = from table in Series.First().Tables
                                        from game in new[] { table.Game1, table.Game2 }
                                        select game.Player;
            string[] playerIds = query.ToArray();
            HashSet<string> playerIdsWithoutReserve = new(playerIds);
            IEnumerable<string> restQuery = from serie in Series
                                            from table in serie.Tables
                                            from game in new[] { table.Game1, table.Game2 }
                                            where playerIdsWithoutReserve.Contains(game.Player) == false
                                            select game.Player;
            List<string> allPlayerIds = playerIds.Concat(
                new HashSet<string>(restQuery).Where(x => playerIdsWithoutReserve.Contains(x) == false)).ToList();
            return allPlayerIds;
        }
    }
}
