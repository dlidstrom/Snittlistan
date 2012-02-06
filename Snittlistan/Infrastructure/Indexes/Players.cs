namespace Snittlistan.Infrastructure.Indexes
{
    using System.Linq;
    using Raven.Client.Indexes;
    using Snittlistan.Models;

    public class Players : AbstractIndexCreationTask<Match8x4, Players.Result>
    {
        public Players()
        {
            Map = matches => from match in matches
                             from team in match.Teams
                             from serie in team.Series
                             from table in serie.Tables
                             from game in table.Games
                             select new
                             {
                                 Player = game.Player,
                             };

            Reduce = results => from result in results
                                group result by result.Player into g
                                select new
                                {
                                    Player = g.Key
                                };
        }

        public class Result
        {
            public string Player { get; set; }
        }
    }
}