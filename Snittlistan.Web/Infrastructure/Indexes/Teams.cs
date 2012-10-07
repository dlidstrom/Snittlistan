namespace Snittlistan.Web.Infrastructure.Indexes
{
    using System.Linq;

    using Raven.Client.Indexes;

    using Snittlistan.Web.Areas.V1.Models;
    using Snittlistan.Web.Models;

    public class Teams : AbstractMultiMapIndexCreationTask<Teams.Result>
    {
        public Teams()
        {
            this.AddMap<Match8x4>(matches => from match in matches
                                             from team in match.Teams
                                             select new { Team = team.Name });

            this.AddMap<Match4x4>(matches => from match in matches
                                             from team in match.Teams
                                             select new { Team = team.Name });

            this.Reduce = results => from result in results
                                     group result by result.Team into g
                                     select new Result { Team = g.Key };
        }

        public class Result
        {
            public string Team { get; set; }
        }
    }
}