namespace Snittlistan.Web.Areas.V2.ViewModels
{
    using System.Collections.Generic;

    public class ResultViewModel
    {
        public int Turn { get; set; }

        public List<TurnResultViewModel> Results { get; set; }
    }
}