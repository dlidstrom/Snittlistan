namespace Snittlistan.Web.Areas.V2.Domain.Match
{
    public class PinsAndScoreResult
    {
        public PinsAndScoreResult(int pins, int score, int serieNumber)
        {
            Pins = pins;
            Score = score;
            SerieNumber = serieNumber;
        }

        public int Pins { get; set; }

        public int Score { get; set; }

        public int SerieNumber { get; set; }
    }
}