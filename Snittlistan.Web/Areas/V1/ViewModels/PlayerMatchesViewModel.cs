namespace Snittlistan.Web.Areas.V1.ViewModels
{
    using System.Collections.Generic;
    using Snittlistan.Web.Infrastructure.Indexes;

    public class PlayerMatchesViewModel
    {
        public string Player { get; set; }

        public List<Player_ByMatch.Result> Stats { get; set; }

        public Matches_PlayerStats.Result Results { get; set; }
    }
}