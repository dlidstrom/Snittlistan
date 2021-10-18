namespace Snittlistan.Queue.Messages
{
    public class MatchRegisteredTask : ITask
    {
        public MatchRegisteredTask(string rosterId, int bitsMatchId, int score, int opponentScore)
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

        public BusinessKey BusinessKey => new(GetType(), $"{RosterId}/{BitsMatchId}");
    }
}
