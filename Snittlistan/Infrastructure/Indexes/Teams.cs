namespace Snittlistan.Infrastructure.Indexes
{
    using System.Linq;
    using Raven.Client.Indexes;
    using Snittlistan.Models;

    public class Teams : AbstractMultiMapIndexCreationTask<Teams.Result>
    {
        public Teams()
        {
            AddMap<Match8x4>(matches => from match in matches
                                        from team in match.Teams
                                        select new { Team = team.Name });

            AddMap<Match4x4>(matches => from match in matches
                                        from team in match.Teams
                                        select new { Team = team.Name });

            Reduce = results => from result in results
                                group result by result.Team into g
                                select new Result { Team = g.Key };
        }

        public class Result
        {
            public string Team { get; set; }
        }
    }
}