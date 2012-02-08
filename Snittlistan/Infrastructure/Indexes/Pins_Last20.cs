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
                                            game.Player,
                                            match.Date.Date,
                                            game.Pins,
                                            game.Score,
                                            Max = game.Pins,
                                            GamesWithStats = game.Strikes != null ? 1 : 0,
                                            TotalStrikes = game.Strikes,
                                            TotalMisses = game.Misses,
                                            TotalOnePinMisses = game.OnePinMisses,
                                            TotalSplits = game.Splits
                                        });

            AddMap<Match8x4>(matches => from match in matches
                                        from team in match.Teams
                                        from serie in team.Series
                                        from table in serie.Tables
                                        from game in table.Games
                                        select new
                                        {
                                            game.Player,
                                            match.Date.Date,
                                            game.Pins,
                                            table.Score,
                                            Max = game.Pins,
                                            GamesWithStats = 1,
                                            TotalStrikes = game.Strikes,
                                            TotalMisses = game.Misses,
                                            TotalOnePinMisses = game.OnePinMisses,
                                            TotalSplits = game.Splits
                                        });

            Reduce = results => from result in results
                                group result by result.Player into g
                                select new Result
                                {
                                    Player = g.Key,
                                    Date = g.OrderByDescending(x => x.Date).Take(20).First().Date,
                                    Pins = g.OrderByDescending(x => x.Date).Take(20).Average(x => x.Pins),
                                    Score = g.OrderByDescending(x => x.Date).Take(20).Average(x => x.Score),
                                    Max = g.OrderByDescending(x => x.Date).Take(20).Max(x => x.Max),
                                    GamesWithStats = g.OrderByDescending(x => x.Date).Take(20).Sum(x => x.GamesWithStats),
                                    TotalStrikes = g.OrderByDescending(x => x.Date).Take(20).Sum(x => x.TotalStrikes),
                                    TotalMisses = g.OrderByDescending(x => x.Date).Take(20).Sum(x => x.TotalMisses),
                                    TotalOnePinMisses = g.OrderByDescending(x => x.Date).Take(20).Sum(x => x.TotalOnePinMisses),
                                    TotalSplits = g.OrderByDescending(x => x.Date).Take(20).Sum(x => x.TotalSplits)
                                };
        }

        public class Result
        {
            public string Player { get; set; }
            public DateTime Date { get; set; }
            public double Score { get; set; }
            public double Pins { get; set; }
            public int Max { get; set; }
            public int GamesWithStats { get; set; }

            public double TotalStrikes { get; set; }
            public double TotalMisses { get; set; }
            public double TotalOnePinMisses { get; set; }
            public double TotalSplits { get; set; }

            public double AverageStrikes { get { return TotalStrikes / Math.Max(1, GamesWithStats); } }
            public double AverageMisses { get { return TotalMisses / Math.Max(1, GamesWithStats); } }
            public double AverageOnePinMisses { get { return TotalOnePinMisses / Math.Max(1, GamesWithStats); } }
            public double AverageSplits { get { return TotalSplits / Math.Max(1, GamesWithStats); } }
        }
    }
}