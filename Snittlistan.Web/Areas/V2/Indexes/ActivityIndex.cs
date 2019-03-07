namespace Snittlistan.Web.Areas.V2.Indexes
{
    using System.Linq;
    using Domain;
    using Raven.Client.Indexes;

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
}