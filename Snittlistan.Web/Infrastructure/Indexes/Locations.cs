namespace Snittlistan.Web.Infrastructure.Indexes
{
    using System.Linq;

    using Raven.Client.Indexes;

    using Snittlistan.Web.Areas.V1.Models;
    using Snittlistan.Web.Models;

    public class Locations : AbstractIndexCreationTask<Match8x4, Locations.Result>
    {
        public Locations()
        {
            this.Map = matches => from match in matches
                             select new { match.Location };

            this.Reduce = results => from result in results
                                group result by result.Location into g
                                select new Result
                                {
                                    Location = g.Key
                                };
        }

        public class Result
        {
            public string Location { get; set; }
        }
    }
}