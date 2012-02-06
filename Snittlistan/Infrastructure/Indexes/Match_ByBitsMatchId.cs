namespace Snittlistan.Infrastructure.Indexes
{
    using System.Linq;
    using Raven.Abstractions.Indexing;
    using Raven.Client.Indexes;
    using Snittlistan.Models;

    public class Match_ByBitsMatchId : AbstractIndexCreationTask<Match8x4, Match_ByBitsMatchId.Result>
    {
        public Match_ByBitsMatchId()
        {
            Map = matches => from match in matches
                             select new { BitsMatchId = match.BitsMatchId };

            Reduce = results => from result in results
                                group result by result.BitsMatchId into g
                                select new
                                {
                                    BitsMatchId = g.Key
                                };
        }

        public class Result
        {
            public int BitsMatchId { get; set; }
        }
    }
}