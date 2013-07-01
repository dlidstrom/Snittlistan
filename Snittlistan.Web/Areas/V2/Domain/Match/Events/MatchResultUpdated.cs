using System.Collections.Generic;
using EventStoreLite;

namespace Snittlistan.Web.Areas.V2.Domain.Match.Events
{
    public class MatchResultUpdated : Event
    {
        public MatchResultUpdated(
            string rosterId,
            List<string> rosterPlayers,
            int teamScore,
            int opponentScore,
            int bitsMatchId,
            string oldRosterId,
            int oldTeamScore,
            int oldOpponentScore,
            int oldBitsMatchId)
        {
            NewRosterId = rosterId;
            NewTeamScore = teamScore;
            NewOpponentScore = opponentScore;
            NewBitsMatchId = bitsMatchId;
            OldRosterId = oldRosterId;
            OldTeamScore = oldTeamScore;
            OldOpponentScore = oldOpponentScore;
            OldBitsMatchId = oldBitsMatchId;
            RosterPlayers = rosterPlayers;
        }

        public string NewRosterId { get; private set; }

        public int NewTeamScore { get; private set; }

        public int NewOpponentScore { get; private set; }

        public int NewBitsMatchId { get; private set; }

        public string OldRosterId { get; private set; }

        public int OldTeamScore { get; private set; }

        public int OldOpponentScore { get; private set; }

        public int OldBitsMatchId { get; private set; }

        public List<string> RosterPlayers { get; private set; }
    }
}