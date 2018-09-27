namespace Snittlistan.Web.Areas.V2.Domain.Match.Commentary
{
    public class PlayerPin
    {
        public PlayerPin(
            string playerId,
            double average,
            int score,
            int pins,
            int seriesPlayed)
        {
            PlayerId = playerId;
            Average = average;
            Score = score;
            Pins = pins;
            SeriesPlayed = seriesPlayed;
        }

        public string PlayerId { get; }

        public double Average { get; }

        public int Score { get; }

        public int Pins { get; }

        public int SeriesPlayed { get; }
    }
}