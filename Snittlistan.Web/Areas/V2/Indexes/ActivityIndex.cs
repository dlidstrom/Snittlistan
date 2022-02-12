#nullable enable

using Raven.Client.Documents.Indexes;
using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Web.Areas.V2.Indexes;

public class ActivityIndex : AbstractIndexCreationTask<Activity>
{
    public ActivityIndex()
    {
        Map = activities => from activity in activities
                            select new
                            {
                                activity.Season
                            };
    }
}
