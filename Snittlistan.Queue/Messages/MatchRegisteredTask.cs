namespace Snittlistan.Queue.Messages
{
    public class MatchRegisteredTask
    {
        public MatchRegisteredTask(string rosterId, int score, int opponentScore)
        {
            RosterId = rosterId;
            Score = score;
            OpponentScore = opponentScore;
        }

        public string RosterId { get; }

        public int Score { get; }

        public int OpponentScore { get; }
    }
}