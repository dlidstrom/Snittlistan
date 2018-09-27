using System.Collections.Generic;
using System.Linq;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public static class ExtensionMethods
    {
        private static readonly Dictionary<string, int> TeamLevelSortOrder = new Dictionary<string, int>
            {
                { "A", 1 },
                { "F", 2 },
                { "B", 3 },
                { "C", 4 }
            };

        public static IEnumerable<RosterViewModel> SortRosters(this IEnumerable<RosterViewModel> rosters)
        {
            return rosters.OrderBy(r => TeamLevelSortOrder[r.Header.TeamLevel]).ThenBy(r => r.Header.Date);
        }

        public static IEnumerable<ResultHeaderViewModel> SortResults(this IEnumerable<ResultHeaderViewModel> results)
        {
            return results.OrderBy(r => TeamLevelSortOrder[r.TeamLevel.ToUpper()]).ThenBy(r => r.Date);
        }
    }
}