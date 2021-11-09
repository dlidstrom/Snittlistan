#nullable enable

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    public static class ExtensionMethods
    {
        private static readonly Dictionary<string, int> TeamLevelSortOrder = new()
        {
            { "A", 1 },
            { "F", 2 },
            { "F1", 3 },
            { "F2", 4 },
            { "B", 5 },
            { "C", 6 },
            { "D", 7 },
            { "DB", 8 },
            { "DC", 9 },
        };

        public static IEnumerable<RosterViewModel> SortRosters(this IEnumerable<RosterViewModel> rosters)
        {
            return rosters.OrderBy(GetSortKey).ThenBy(r => r.Header!.Date);

            static (int i, string s) GetSortKey(RosterViewModel vm)
            {
                if (TeamLevelSortOrder.TryGetValue(vm.Header!.TeamLevel!, out int i))
                {
                    return (i, string.Empty);
                }

                return (-1, vm.Header.Team!);
            }
        }

        public static IEnumerable<ResultHeaderViewModel> SortResults(this IEnumerable<ResultHeaderViewModel> results)
        {
            return results.OrderBy(GetSortKey).ThenBy(r => r.Date);

            static (int i, string s) GetSortKey(ResultHeaderViewModel vm)
            {
                if (TeamLevelSortOrder.TryGetValue(vm.TeamLevel.ToUpper(), out int i))
                {
                    return (i, string.Empty);
                }

                return (-1, vm.Team);
            }
        }
    }
}
