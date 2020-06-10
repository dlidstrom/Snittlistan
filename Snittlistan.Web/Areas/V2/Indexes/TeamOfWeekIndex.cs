namespace Snittlistan.Web.Areas.V2.Indexes
{
    using System.Linq;
    using Raven.Client.Indexes;
    using Snittlistan.Web.Areas.V2.ReadModels;

    public class TeamOfWeekIndex : AbstractIndexCreationTask<TeamOfWeek>
    {
        public TeamOfWeekIndex()
        {
            Map = weeks => from week in weeks
                           select new
                           {
                               week.Season
                           };
        }
    }
}