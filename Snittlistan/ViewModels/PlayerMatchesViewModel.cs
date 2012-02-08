namespace Snittlistan.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Snittlistan.Infrastructure.Indexes;

    public class PlayerMatchesViewModel
    {
        public string Player { get; set; }
        public List<Player_ByMatch.Result> Stats { get; set; }
        public Matches_PlayerStats.Result Results { get; set; }
        public Pins_Last20.Result Last20 { get; set; }
        public double AverageStrikes
        {
            get
            {
                return Results.AverageStrikes;
            }
        }

        public double AverageMisses
        {
            get
            {
                return Results.AverageMisses;
            }
        }
    }
}