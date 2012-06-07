namespace Snittlistan.Infrastructure.Indexes
{
    using System.Linq;
    using Models;
    using Raven.Client.Indexes;

    public class Match_ByBitsMatchId : AbstractIndexCreationTask<Match8x4, Match_ByBitsMatchId.Result>
    {
        public Match_ByBitsMatchId()
        {
            Map = matches => from match in matches
                             select new { match.BitsMatchId };
        }

        public class Result
        {
            public int BitsMatchId { get; set; }
        }
    }
}