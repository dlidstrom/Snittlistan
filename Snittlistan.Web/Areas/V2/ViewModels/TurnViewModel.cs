namespace Snittlistan.Web.Areas.V2.ViewModels
{
    using System;
    using System.Collections.Generic;

    public class TurnViewModel
    {
        public int Turn { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<RosterViewModel> Rosters { get; set; }
    }
}