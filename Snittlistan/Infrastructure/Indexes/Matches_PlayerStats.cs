namespace Snittlistan.Infrastructure.Indexes
{
    using System;
    using System.Linq;
    using Raven.Client.Indexes;
    using Snittlistan.Models;

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
                                            Player = game.Player,
                                            Pins = game.Pins,
                                            Series = 1,
                                            Score = table.Score,
                                            Max = game.Pins,
                                            GamesWithStats = 1,
                                            Strikes = game.Strikes,
                                            Misses = game.Misses,
                                            OnePinMisses = game.OnePinMisses,
                                            Splits = game.Splits,
                                            CoveredAll = game.CoveredAll ? 1 : 0
                                        });

            AddMap<Match4x4>(matches => from match in matches
                                        from team in match.Teams
                                        from serie in team.Series
                                        from game in serie.Games
                                        select new
                                        {
                                            Player = game.Player,
                                            Pins = game.Pins,
                                            Series = 1,
                                            Score = game.Score,
                                            Max = game.Pins,
                                            GamesWithStats = game.Strikes != null ? 1 : 0,
                                            Strikes = game.Strikes,
                                            Misses = game.Misses,
                                            OnePinMisses = game.OnePinMisses,
                                            Splits = game.Splits,
                                            CoveredAll = game.CoveredAll ? 1 : 0
                                        });

            Reduce = results => from result in results
                                group result by result.Player into stat
                                select new
                                {
                                    Player = stat.Key,
                                    Pins = stat.Sum(s => s.Pins),
                                    Series = stat.Sum(s => s.Series),
                                    Score = stat.Sum(s => s.Score),
                                    Max = stat.Max(s => s.Max),
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
            public DateTimeOffset Date { get; set; }
            public double Pins { get; set; }
            public double Series { get; set; }
            public double Score { get; set; }
            public int Max { get; set; }
            public int GamesWithStats { get; set; }
            public double Strikes { get; set; }
            public double Misses { get; set; }
            public double OnePinMisses { get; set; }
            public double Splits { get; set; }
            public int CoveredAll { get; set; }

            public double AverageScore { get { return Score / Series; } }
            public double AveragePins { get { return Pins / Series; } }
            public double AverageStrikes { get { return Strikes / GamesWithStats; } }
            public double AverageMisses { get { return Misses / GamesWithStats; } }
            public double AverageOnePinMisses { get { return OnePinMisses / GamesWithStats; } }
            public double AverageSplits { get { return Splits / GamesWithStats; } }
        }
    }
}