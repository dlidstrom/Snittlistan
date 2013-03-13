using EventStoreLite;

namespace Snittlistan.Web.Areas.V2.Domain.Match.Events
{
    public class MatchResultRegistered : Event
    {
        public string RosterId { get; set; }

        public int TeamScore { get; set; }

        public int OpponentScore { get; set; }

        public int BitsMatchId { get; set; }

        public MatchResultRegistered(string rosterId, int teamScore, int opponentScore, int bitsMatchId)
        {
            this.RosterId = rosterId;
            this.TeamScore = teamScore;
            this.OpponentScore = opponentScore;
            this.BitsMatchId = bitsMatchId;
        }
    }
}