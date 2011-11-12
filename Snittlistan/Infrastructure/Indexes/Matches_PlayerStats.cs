namespace Snittlistan.Infrastructure.Indexes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
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
            public double Pins { get; set; }
            public int Series { get; set; }
            public int Score { get; set; }
            public int Max { get; set; }
            public int Strikes { get; set; }
            public int Misses { get; set; }
            public int OnePinMisses { get; set; }
            public int Splits { get; set; }
            public double Average { get; set; }
            public int CoveredAll { get; set; }
        }
    }
}