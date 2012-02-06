namespace Snittlistan.Infrastructure.Indexes
{
    using System;
    using System.Linq;
    using Raven.Abstractions.Indexing;
    using Raven.Client.Indexes;
    using Snittlistan.Models;

    public class Pins_Last20 : AbstractMultiMapIndexCreationTask<Pins_Last20.Result>
    {
        public Pins_Last20()
        {
            AddMap<Match4x4>(matches => from match in matches
                                        from team in match.Teams
                                        from serie in team.Series
                                        from game in serie.Games
                                        select new
                                        {
                                            Player = game.Player,
                                            Date = match.Date,
                                            Pins = game.Pins,
                                            Score = game.Score,
                                            Max = game.Pins,
                                            Strikes = game.Strikes,
                                            Misses = game.Misses,
                                            OnePinMisses = game.OnePinMisses,
                                            Splits = game.Splits
                                        });

            AddMap<Match8x4>(matches => from match in matches
                                        from team in match.Teams
                                        from serie in team.Series
                                        from table in serie.Tables
                                        from game in table.Games
                                        select new
                                        {
                                            Player = game.Player,
                                            Date = match.Date,
                                            Pins = game.Pins,
                                            Score = table.Score,
                                            Max = game.Pins,
                                            Strikes = game.Strikes,
                                            Misses = game.Misses,
                                            OnePinMisses = game.OnePinMisses,
                                            Splits = game.Splits
                                        });

            Reduce = results => from result in results
                                group result by result.Player into g
                                select new
                                {
                                    Player = g.Key,
                                    Date = g.OrderByDescending(x => x.Date).Take(20).First().Date,
                                    Pins = g.OrderByDescending(x => x.Date).Take(20).Average(x => x.Pins),
                                    Score = g.OrderByDescending(x => x.Date).Take(20).Average(x => x.Score),
                                    Max = g.OrderByDescending(x => x.Date).Take(20).Max(x => x.Max),
                                    Strikes = g.OrderByDescending(x => x.Date).Take(20).Average(x => x.Strikes),
                                    Misses = g.OrderByDescending(x => x.Date).Take(20).Average(x => x.Misses),
                                    OnePinMisses = g.OrderByDescending(x => x.Date).Take(20).Average(x => x.OnePinMisses),
                                    Splits = g.OrderByDescending(x => x.Date).Take(20).Average(x => x.Splits)
                                };
        }

        public class Result
        {
            public string Player { get; set; }
            public string Date { get; set; }
            public double Score { get; set; }
            public double Pins { get; set; }
            public int Max { get; set; }
            public double Strikes { get; set; }
            public double Misses { get; set; }
            public double OnePinMisses { get; set; }
            public double Splits { get; set; }
        }
    }
}