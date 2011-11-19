namespace Snittlistan.ViewModels
{
    using System.Collections.Generic;
    using Snittlistan.Infrastructure.Indexes;

    public class PlayerMatchesViewModel
    {
        public string Player { get; set; }
        public List<Player_ByMatch.Result> Stats { get; set; }
    }
}