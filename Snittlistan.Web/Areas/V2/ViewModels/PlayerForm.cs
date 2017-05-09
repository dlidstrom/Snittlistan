namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class PlayerFormViewModel
    {
        public PlayerFormViewModel(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public double Last5Average { get; set; }

        public double SeasonAverage { get; set; }

        public int TotalScore { get; set; }

        public double ScoreAverage { get; set; }

        public bool HasResult { get; set; }

        public int TotalSeries { get; set; }

        public string FormattedSeasonAverage()
        {
            if (HasResult == false) return string.Empty;
            return SeasonAverage.ToString("0.0");
        }

        public string FormattedLast5Average()
        {
            return Last5Average.ToString("0.0");
        }

        public string FormattedDiff()
        {
            if (HasResult == false) return string.Empty;
            var diff = Last5Average - SeasonAverage;
            return diff.ToString("+0.0;-0.0;0");
        }

        public string FormattedScoreAverage()
        {
            return ScoreAverage.ToString("0%");
        }

        public string Class()
        {
            if (HasResult == false) return string.Empty;
            var diff = Last5Average - SeasonAverage;
            string klass;
            if (diff <= -10)
            {
                klass = "form-minus-10";
            }
            else if (diff <= -6)
            {
                klass = "form-minus-6";
            }
            else if (diff <= -4)
            {
                klass = "form-minus-4";
            }
            else if (diff <= -2)
            {
                klass = "form-minus-2";
            }
            else if (diff >= 10)
            {
                klass = "form-plus-10";
            }
            else if (diff >= 6)
            {
                klass = "form-plus-6";
            }
            else if (diff >= 4)
            {
                klass = "form-plus-4";
            }
            else if (diff >= 2)
            {
                klass = "form-plus-2";
            }
            else
            {
                klass = "form-0";
            }

            return klass;
        }
    }
}