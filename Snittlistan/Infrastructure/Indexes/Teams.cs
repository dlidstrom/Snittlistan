namespace Snittlistan.Infrastructure.Indexes
{
    using System.Linq;
    using Raven.Client.Indexes;
    using Snittlistan.Models;

    public class Teams : AbstractIndexCreationTask<Match, Teams.Result>
    {
        public Teams()
        {
            Map = matches => from match in matches
                             from team in match.Teams
                             select new
                             {
                                 Team = team.Name,
                             };

            Reduce = results => from result in results
                                group result by result.Team into g
                                select new
                                {
                                    Team = g.Key
                                };
        }

        public class Result
        {
            public string Team { get; set; }
        }
    }
}