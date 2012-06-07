namespace Snittlistan.Infrastructure.Indexes
{
    using System.Linq;
    using Models;
    using Raven.Client.Indexes;

    public class Players : AbstractMultiMapIndexCreationTask<Players.Result>
    {
        public Players()
        {
            AddMap<Match8x4>(matches => from match in matches
                                        from team in match.Teams
                                        from serie in team.Series
                                        from table in serie.Tables
                                        from game in table.Games
                                        select new { game.Player });

            AddMap<Match4x4>(matches => from match in matches
                                        from team in match.Teams
                                        from serie in team.Series
                                        from game in serie.Games
                                        select new { game.Player });

            Reduce = results => from result in results
                                group result by result.Player into g
                                select new Result { Player = g.Key };
        }

        public class Result
        {
            public string Player { get; set; }
        }
    }
}