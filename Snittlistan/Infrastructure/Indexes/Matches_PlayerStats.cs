namespace Snittlistan.Infrastructure.Indexes
{
    using System;
    using System.Linq;
    using Raven.Client.Indexes;
    using Snittlistan.Models;

    public class Matches_PlayerStats : AbstractIndexCreationTask<Match, Matches_PlayerStats.Result>
    {
        public Matches_PlayerStats()
        {
            Map = matches => from match in matches
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
                                 Strikes = game.Strikes,
                                 Misses = game.Misses,
                                 OnePinMisses = game.OnePinMisses,
                                 Splits = game.Splits,
                                 Average = game.Pins,
                                 CoveredAll = game.CoveredAll ? 1 : 0
                             };

            Reduce = results => from result in results
                                group result by result.Player into stat
                                select new
                                {
                                    Player = stat.Key,
                                    Pins = stat.Sum(s => s.Pins),
                                    Series = stat.Sum(s => s.Series),
                                    Score = stat.Sum(s => s.Score),
                                    Max = stat.Max(s => s.Max),
                                    Strikes = stat.Sum(s => s.Strikes),
                                    Misses = stat.Sum(s => s.Misses),
                                    OnePinMisses = stat.Sum(s => s.OnePinMisses),
                                    Splits = stat.Sum(s => s.Splits),
                                    Average = stat.Sum(s => s.Pins) / stat.Sum(s => s.Series),
                                    CoveredAll = stat.Sum(s => s.CoveredAll)
                                };
        }

        public class Result
        {
            public string Player { get; set; }
            public DateTime Date { get; set; }
            public double Pins { get; set; }
            public int Series { get; set; }
            public double Score { get; set; }
            public int Max { get; set; }
            public double Strikes { get; set; }
            public double Misses { get; set; }
            public double OnePinMisses { get; set; }
            public double Splits { get; set; }
            public double Average { get; set; }
            public int CoveredAll { get; set; }

            public double AverageScore { get { return Score / Series; } }
            public double AveragePins { get { return Pins / Series; } }
            public double AverageStrikes { get { return Strikes / Series; } }
            public double AverageMisses { get { return Misses / Series; } }
            public double AverageOnePinMisses { get { return OnePinMisses / Series; } }
            public double AverageSplits { get { return Splits / Series; } }
        }
    }
}