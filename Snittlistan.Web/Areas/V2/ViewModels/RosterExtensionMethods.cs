using System.Collections.Generic;
using System.Linq;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public static class RosterExtensionMethods
    {
        private static readonly Dictionary<char, int> TeamLevelSortOrder = new Dictionary<char, int>
            {
                { 'a', 1 },
                { 'f', 2 },
                { 'b', 3 },
                { 'c', 4 }
            };

        public static IEnumerable<RosterViewModel> SortRosters(this IEnumerable<RosterViewModel> rosters)
        {
            return rosters.OrderBy(r => TeamLevelSortOrder[r.TeamLevel]).ThenBy(r => r.Date);
        }
    }
}