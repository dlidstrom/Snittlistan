namespace Snittlistan.ViewModels
{
    public class PlayerStatsViewModel
    {
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