using System.Collections.Generic;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class InitialDataViewModel
    {
        public List<TurnViewModel> Turns { get; set; }

        public int SeasonStart { get; set; }

        public bool IsFiltered { get; set; }
    }
}