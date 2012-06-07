namespace Snittlistan.ViewModels
{
    using System.Collections.Generic;
    using Infrastructure.Indexes;

    public class PlayerStatsViewModel
    {
        private readonly Matches_PlayerStats.Result result;

        public PlayerStatsViewModel(Matches_PlayerStats.Result result, IDictionary<string, double> last20)
        {
            this.result = result;
            double averageLast20;
            last20.TryGetValue(result.Player, out averageLast20);
            AverageLast20 = averageLast20;
        }

        public string Player { get { return result.Player; } }
        public double Series { get { return result.Series; } }
        public double AverageScore { get { return result.AverageScore; } }
        public double AveragePins { get { return result.AveragePins; } }
        public int BestGame { get { return result.BestGame; } }
        public double AverageStrikes { get { return result.AverageStrikes; } }
        public double AverageMisses { get { return result.AverageMisses; } }
        public double AverageOnePinMisses { get { return result.AverageOnePinMisses; } }
        public double AverageSplits { get { return result.AverageSplits; } }
        public int CoveredAll { get { return result.CoveredAll; } }
        public double AverageLast20 { get; private set; }
    }
}