namespace Snittlistan.Web.Areas.V2.Models
{
    public class MatchResult
    {
        public MatchResult(string rosterId, int teamScore, int opponentScore, int bitsMatchId)
        {
            RosterId = rosterId;
            TeamScore = teamScore;
            OpponentScore = opponentScore;
            BitsMatchId = bitsMatchId;
        }

        public int Id { get; set; }

        public string RosterId { get; private set; }

        public int TeamScore { get; set; }

        public int OpponentScore { get; set; }

        public int BitsMatchId { get; set; }
    }
}