namespace Snittlistan.Web.Areas.V2.ViewModels
{
    using System.Collections.Generic;

    public class PlayerDataViewModel
    {
        public List<PlayerViewModel> Players { get; set; }

        public int SeasonStart { get; set; }
        public int SeasonEnd { get; set; }
    }
}