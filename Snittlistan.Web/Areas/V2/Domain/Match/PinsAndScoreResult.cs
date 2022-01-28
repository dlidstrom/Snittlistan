namespace Snittlistan.Web.Areas.V2.Domain.Match;

public class PinsAndScoreResult
{
    public PinsAndScoreResult(int pins, int score, int serieNumber)
    {
        Pins = pins;
        Score = score;
        SerieNumber = serieNumber;
    }

    public int Pins { get; private set; }

    public int Score { get; private set; }

    public int SerieNumber { get; private set; }
}
