#nullable enable

namespace Snittlistan.Web.Infrastructure.Indexes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Raven.Client.Indexes;
    using Snittlistan.Web.Areas.V1.Models;

    public class Matches_PlayerStats : AbstractMultiMapIndexCreationTask<Matches_PlayerStats.Result>
    {
        public Matches_PlayerStats()
        {
            AddMap<Match8x4>(matches => from match in matches
                                        from team in match.Teams
                                        from serie in team.Series
                                        from table in serie.Tables
                                        from game in table.Games
                                        select new
                                        {
                                            game.Player,
                                            game.Pins,
                                            MinDate = match.Date,
                                            Last20Games = Enumerable.Repeat(new
                                            {
                                                match.Date,
                                                game.Pins,
                                                game.Player
                                            }, 1),
                                            Last20GamesAvg = game.Pins,
                                            Series = 1,
                                            table.Score,
                                            BestGame = game.Pins,
                                            GamesWithStats = game.Strikes != null ? 1 : 0,
                                            Strikes = game.Strikes ?? 0,
                                            Misses = game.Misses ?? 0,
                                            OnePinMisses = game.OnePinMisses ?? 0,
                                            Splits = game.Splits ?? 0,
                                            CoveredAll = game.CoveredAll ? 1 : 0
                                        });

            AddMap<Match4x4>(matches => from match in matches
                                        from team in match.Teams
                                        from serie in team.Series
                                        from game in serie.Games
                                        select new
                                        {
                                            game.Player,
                                            game.Pins,
                                            MinDate = match.Date,
                                            Last20Games = Enumerable.Repeat(new
                                            {
                                                match.Date,
                                                game.Pins,
                                                game.Player
                                            }, 1),
                                            Last20GamesAvg = game.Pins,
                                            Series = 1,
                                            game.Score,
                                            BestGame = game.Pins,
                                            GamesWithStats = game.Strikes != null ? 1 : 0,
                                            Strikes = game.Strikes ?? 0,
                                            Misses = game.Misses ?? 0,
                                            OnePinMisses = game.OnePinMisses ?? 0,
                                            Splits = game.Splits ?? 0,
                                            CoveredAll = game.CoveredAll ? 1 : 0
                                        });

            Reduce = results => from result in results
                                group result by result.Player into stat
                                select new Result
                                {
                                    Player = stat.Key,
                                    Pins = stat.Sum(s => s.Pins),
                                    MinDate = new DateTime(stat.Min(x => x.MinDate.Ticks)),
                                    Last20Games = stat.SelectMany(x => x.Last20Games)
                                        .OrderByDescending(x => x.Date)
                                        .Take(20),
                                    Last20GamesAvg = stat.SelectMany(x => x.Last20Games)
                                        .OrderByDescending(x => x.Date)
                                        .Take(20)
                                        .Average(x => x.Pins),
                                    Series = stat.Sum(s => s.Series),
                                    Score = stat.Sum(s => s.Score),
                                    BestGame = stat.Max(s => s.BestGame),
                                    GamesWithStats = stat.Sum(x => x.GamesWithStats),
                                    Strikes = stat.Sum(s => s.Strikes),
                                    Misses = stat.Sum(s => s.Misses),
                                    OnePinMisses = stat.Sum(s => s.OnePinMisses),
                                    Splits = stat.Sum(s => s.Splits),
                                    CoveredAll = stat.Sum(s => s.CoveredAll)
                                };
        }

        public class Game
        {
            public DateTime Date { get; set; }

            public int Pins { get; set; }
        }

        public class Result
        {
            public string Player { get; set; } = null!;

            public double Pins { get; set; }

            public DateTime MinDate { get; set; }

            public IEnumerable<Game> Last20Games { get; set; } = null!;

            public double Last20GamesAvg { get; set; }

            public double Series { get; set; }

            public double Score { get; set; }

            public int BestGame { get; set; }

            public int GamesWithStats { get; set; }

            public double Strikes { get; set; }

            public double Misses { get; set; }

            public double OnePinMisses { get; set; }

            public double Splits { get; set; }

            public int CoveredAll { get; set; }

            public double AverageScore => Score / Series;

            public double AveragePins => Pins / Series;

            public double AverageStrikes => Strikes / Math.Max(1, GamesWithStats);

            public double AverageMisses => Misses / Math.Max(1, GamesWithStats);

            public double AverageOnePinMisses => OnePinMisses / Math.Max(1, GamesWithStats);

            public double AverageSplits => Splits / Math.Max(1, GamesWithStats);
        }
    }
}
