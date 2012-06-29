namespace Snittlistan.Infrastructure.Indexes
{
    using System;
    using System.Linq;
    using Models;
    using Raven.Client.Indexes;

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
                                            Series = 1,
                                            table.Score,
                                            BestGame = game.Pins,
                                            GamesWithStats = game.Strikes != null ? 1 : 0,
                                            game.Strikes,
                                            game.Misses,
                                            game.OnePinMisses,
                                            game.Splits,
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
                                            Series = 1,
                                            game.Score,
                                            BestGame = game.Pins,
                                            GamesWithStats = game.Strikes != null ? 1 : 0,
                                            game.Strikes,
                                            game.Misses,
                                            game.OnePinMisses,
                                            game.Splits,
                                            CoveredAll = game.CoveredAll ? 1 : 0
                                        });

            Reduce = results => from result in results
                                group result by result.Player into stat
                                select new Result
                                {
                                    Player = stat.Key,
                                    Pins = stat.Sum(s => s.Pins),
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

        public class Result
        {
            public string Player { get; set; }
            public double Pins { get; set; }
            public double Series { get; set; }
            public double Score { get; set; }
            public int BestGame { get; set; }
            public int GamesWithStats { get; set; }
            public double Strikes { get; set; }
            public double Misses { get; set; }
            public double OnePinMisses { get; set; }
            public double Splits { get; set; }
            public int CoveredAll { get; set; }

            public double AverageScore { get { return Score / Series; } }
            public double AveragePins { get { return Pins / Series; } }
            public double AverageStrikes { get { return Strikes / Math.Max(1, GamesWithStats); } }
            public double AverageMisses { get { return Misses / Math.Max(1, GamesWithStats); } }
            public double AverageOnePinMisses { get { return OnePinMisses / Math.Max(1, GamesWithStats); } }
            public double AverageSplits { get { return Splits / Math.Max(1, GamesWithStats); } }
        }
    }
}