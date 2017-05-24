namespace Snittlistan.Web.Tasks
{
    public class MatchRegisteredTask
    {
        public MatchRegisteredTask(string rosterId, int score, int opponentScore)
        {
            RosterId = rosterId;
            Score = score;
            OpponentScore = opponentScore;
        }

        public string RosterId { get; private set; }

        public int Score { get; private set; }

        public int OpponentScore { get; private set; }

        public override string ToString()
        {
            return "RosterId: " + RosterId;
        }
    }
}