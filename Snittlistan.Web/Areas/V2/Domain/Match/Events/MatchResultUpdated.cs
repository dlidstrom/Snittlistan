using EventStoreLite;

namespace Snittlistan.Web.Areas.V2.Domain.Match.Events
{
    public class MatchResultUpdated : Event
    {
        public MatchResultUpdated(
            string rosterId,
            int teamScore,
            int opponentScore,
            int bitsMatchId,
            string oldRosterId,
            int oldTeamScore,
            int oldOpponentScore,
            int oldBitsMatchId)
        {
            this.NewRosterId = rosterId;
            this.NewTeamScore = teamScore;
            this.NewOpponentScore = opponentScore;
            this.NewBitsMatchId = bitsMatchId;
            this.OldRosterId = oldRosterId;
            this.OldTeamScore = oldTeamScore;
            this.OldOpponentScore = oldOpponentScore;
            this.OldBitsMatchId = oldBitsMatchId;
        }

        public string NewRosterId { get; private set; }

        public int NewTeamScore { get; private set; }

        public int NewOpponentScore { get; private set; }

        public int NewBitsMatchId { get; private set; }

        public string OldRosterId { get; private set; }

        public int OldTeamScore { get; private set; }

        public int OldOpponentScore { get; private set; }

        public int OldBitsMatchId { get; private set; }
    }
}