namespace Snittlistan.Web.Areas.V2.Domain.Match.Commentary;

public class PlayerResult
{
    public PlayerResult(string playerId, int pins, int score)
    {
        PlayerId = playerId;
        Pins = pins;
        Score = score;
    }

    public string PlayerId { get; private set; }

    public int Pins { get; private set; }

    public int Score { get; private set; }
}
