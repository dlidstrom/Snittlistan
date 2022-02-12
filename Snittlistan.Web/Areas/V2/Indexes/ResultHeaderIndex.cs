#nullable enable

using Raven.Client.Documents.Indexes;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.Indexes;

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
