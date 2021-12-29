#nullable enable

namespace Snittlistan.Queue.Messages;

public class MatchRegisteredTask : TaskBase
{
    public MatchRegisteredTask(string rosterId, int bitsMatchId, int score, int opponentScore)
        : base(new(typeof(MatchRegisteredTask).FullName, $"{rosterId}/{bitsMatchId}"))
    {
        RosterId = rosterId;
        BitsMatchId = bitsMatchId;
        Score = score;
        OpponentScore = opponentScore;
    }

    public string RosterId { get; }

    public int BitsMatchId { get; }

    public int Score { get; }

    public int OpponentScore { get; }
}
