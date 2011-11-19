namespace Snittlistan.ViewModels
{
    using System.Collections.Generic;
    using Snittlistan.Infrastructure.Indexes;

    public class PlayerStatsViewModel
    {
        public PlayerStatsViewModel(Matches_PlayerStats.Result s, IDictionary<string, double> last20)
        {
            Player = s.Player;
            Series = s.Series;
            AverageScore = s.Score / s.Series;
            AveragePins = s.Pins / s.Series;
            Max = s.Max;
            AverageStrikes = s.Strikes / s.Series;
            AverageMisses = s.Misses / s.Series;
            AverageOnePinMisses = s.OnePinMisses / s.Series;
            AverageSplits = s.Splits / s.Series;
            CoveredAll = s.CoveredAll;
            double averageLast20 = 0.0;
            last20.TryGetValue(s.Player, out averageLast20);
            AverageLast20 = averageLast20;
        }

        public string Player { get; set; }
        public int Series { get; set; }
        public double AverageScore { get; set; }
        public double AveragePins { get; set; }
        public int Max { get; set; }
        public double AverageStrikes { get; set; }
        public double AverageMisses { get; set; }
        public double AverageOnePinMisses { get; set; }
        public double AverageSplits { get; set; }
        public int CoveredAll { get; set; }
        public double AverageLast20 { get; set; }
    }
}