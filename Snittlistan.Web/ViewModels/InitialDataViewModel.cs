namespace Snittlistan.Web.ViewModels
{
    using System.Collections.Generic;

    public class InitialDataViewModel
    {
        public List<TurnViewModel> Turns { get; set; }

        public int SeasonStart { get; set; }
        public int SeasonEnd { get; set; }
    }
}