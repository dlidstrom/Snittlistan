using System.Collections.Generic;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class TeamOfWeekViewModel
    {
        public TeamOfWeekViewModel(int season, List<TeamOfWeek> teamOfWeek)
        {
            Season = season;
            TeamOfWeek = teamOfWeek;
        }

        public int Season { get; private set; }

        public List<TeamOfWeek> TeamOfWeek { get; private set; }
    }
}