using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.Indexes
{
    /*public class MatchResultIndex : AbstractIndexCreationTask<MatchResultReadModel, MatchResultIndex.Result>
    {
        public MatchResultIndex()
        {
            Map = readModels => from result in readModels
                                select new
                                {
                                    result.AggregateId,
                                    result.RosterId,
                                    result.Season,
                                    result.TeamScore,
                                    result.OpponentScore,
                                    result.BitsMatchId
                                };

            Store(x => x.AggregateId, FieldStorage.Yes);
            Store(x => x.RosterId, FieldStorage.Yes);
            Store(x => x.Season, FieldStorage.Yes);
            Store(x => x.TeamScore, FieldStorage.Yes);
            Store(x => x.OpponentScore, FieldStorage.Yes);
            Store(x => x.BitsMatchId, FieldStorage.Yes);
        }

        public class Result
        {
            public string AggregateId { get; set; }

            public string RosterId { get; set; }

            public int Season { get; set; }

            public int TeamScore { get; set; }

            public int OpponentScore { get; set; }

            public int BitsMatchId { get; set; }
        }
    }*/
}