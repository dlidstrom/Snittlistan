using System.Collections.Generic;
using Snittlistan.Web.Infrastructure.Indexes;

namespace Snittlistan.Web.Areas.V1.ViewModels
{
    public class PlayerMatchesViewModel
    {
        public string Player { get; set; }

        public List<Player_ByMatch.Result> Stats { get; set; }

        public Matches_PlayerStats.Result Results { get; set; }
    }
}