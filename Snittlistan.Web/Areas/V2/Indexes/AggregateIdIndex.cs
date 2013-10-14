using System.Linq;
using Raven.Client.Indexes;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.Indexes
{
    public class AggregateIdIndex : AbstractIndexCreationTask<AggregateIdReadModel>
    {
        public AggregateIdIndex()
        {
            Map = results => from result in results
                             select new
                             {
                                 result.AggregateId,
                                 result.Type
                             };
        }
    }
}