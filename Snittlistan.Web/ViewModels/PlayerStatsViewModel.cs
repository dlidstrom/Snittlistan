namespace Snittlistan.Web.ViewModels
{
    using System.Collections.Generic;

    using Snittlistan.Web.Infrastructure.Indexes;

    public class PlayerStatsViewModel
    {
        private readonly Matches_PlayerStats.Result result;

        public PlayerStatsViewModel(Matches_PlayerStats.Result result, IDictionary<string, double> last20)
        {
            this.result = result;
            double averageLast20;
            last20.TryGetValue(result.Player, out averageLast20);
            this.AverageLast20 = averageLast20;
        }

        public string Player { get { return this.result.Player; } }
        public double Series { get { return this.result.Series; } }
        public double AverageScore { get { return this.result.AverageScore; } }
        public double AveragePins { get { return this.result.AveragePins; } }
        public int BestGame { get { return this.result.BestGame; } }
        public double AverageStrikes { get { return this.result.AverageStrikes; } }
        public double AverageMisses { get { return this.result.AverageMisses; } }
        public double AverageOnePinMisses { get { return this.result.AverageOnePinMisses; } }
        public double AverageSplits { get { return this.result.AverageSplits; } }
        public int CoveredAll { get { return this.result.CoveredAll; } }
        public double AverageLast20 { get; private set; }
    }
}