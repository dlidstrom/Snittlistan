namespace Snittlistan.Web.Areas.V2.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Snittlistan.Web.Areas.V2.Indexes;

    public class MatchResultViewModel
    {
        public List<ResultViewModel> Results { get; set; }

        public int SeasonStart { get; set; }

        public int SeasonEnd
        {
            get
            {
                return SeasonStart + 1;
            }
        }
    }
}