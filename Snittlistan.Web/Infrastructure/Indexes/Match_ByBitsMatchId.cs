namespace Snittlistan.Web.Infrastructure.Indexes
{
    using System.Linq;

    using Raven.Client.Indexes;

    using Snittlistan.Web.Areas.V1.Models;
    using Snittlistan.Web.Models;

    public class Match_ByBitsMatchId : AbstractIndexCreationTask<Match8x4, Match_ByBitsMatchId.Result>
    {
        public Match_ByBitsMatchId()
        {
            this.Map = matches => from match in matches
                             select new { match.BitsMatchId };
        }

        public class Result
        {
            public int BitsMatchId { get; set; }
        }
    }
}