namespace Snittlistan.Infrastructure.Indexes
{
    using System;
    using System.Linq;
    using Raven.Abstractions.Indexing;
    using Raven.Client.Indexes;
    using Snittlistan.Models;

    public class Pins_Last20 : AbstractIndexCreationTask<Match, Pins_Last20.Result>
    {
        public Pins_Last20()
        {
            Map = matches => from match in matches
                             from team in match.Teams
                             from serie in team.Series
                             from table in serie.Tables
                             from game in table.Games
                             select new
                             {
                                 Player = game.Player,
                                 Date = match.Date,
                                 Pins = game.Pins
                             };

            Reduce = results => from result in results
                                group result by result.Player into g
                                select new
                                {
                                    Player = g.Key,
                                    Date = g.OrderByDescending(x => x.Date).Take(20).First().Date,
                                    Pins = g.OrderByDescending(x => x.Date).Take(20).Average(x => x.Pins)
                                };
        }

        public class Result
        {
            public string Player { get; set; }
            public string Date { get; set; }
            public double Pins { get; set; }
        }
    }
}