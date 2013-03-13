using System.Collections.Generic;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class MatchResultViewModel
    {
        public MatchResultViewModel()
        {
            Turns = new Dictionary<int, List<ResultHeaderReadModel>>();
        }

        public int SeasonStart { get; set; }

        public int SeasonEnd
        {
            get
            {
                return SeasonStart + 1;
            }
        }

        public Dictionary<int, List<ResultHeaderReadModel>> Turns { get; set; }
    }
}