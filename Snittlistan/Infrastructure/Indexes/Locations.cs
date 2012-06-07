namespace Snittlistan.Infrastructure.Indexes
{
    using System.Linq;
    using Models;
    using Raven.Client.Indexes;

    public class Locations : AbstractIndexCreationTask<Match8x4, Locations.Result>
    {
        public Locations()
        {
            Map = matches => from match in matches
                             select new { match.Location };

            Reduce = results => from result in results
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