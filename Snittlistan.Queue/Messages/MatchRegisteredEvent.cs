namespace Snittlistan.Queue.Messages
{
    public class MatchRegisteredEvent
    {
        public MatchRegisteredEvent(string rosterId, int score, int opponentScore)
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