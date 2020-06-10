namespace Snittlistan.Web.Areas.V2.Indexes
{
    using System.Linq;
    using Raven.Client.Indexes;
    using Snittlistan.Web.Areas.V2.ReadModels;

    public class ResultHeaderIndex : AbstractIndexCreationTask<ResultHeaderReadModel>
    {
        public ResultHeaderIndex()
        {
            Map = models => from model in models
                            select new
                            {
                                model.Id,
                                model.Season
                            };
        }
    }
}