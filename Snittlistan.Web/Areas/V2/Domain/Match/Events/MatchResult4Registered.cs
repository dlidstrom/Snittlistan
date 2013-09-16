using System;
using System.Collections.Generic;
using EventStoreLite;

namespace Snittlistan.Web.Areas.V2.Domain.Match.Events
{
    public class MatchResult4Registered : Event
    {
        public MatchResult4Registered(
            string rosterId,
            List<string> rosterPlayers,
            int teamScore,
            int opponentScore,
            int bitsMatchId)
        {
            if (rosterId == null)
                throw new ArgumentNullException("rosterId");
            if (rosterPlayers == null)
                throw new ArgumentNullException("rosterPlayers");

            RosterId = rosterId;
            RosterPlayers = rosterPlayers;
            TeamScore = teamScore;
            OpponentScore = opponentScore;
            BitsMatchId = bitsMatchId;
        }

        public string RosterId { get; private set; }

        public List<string> RosterPlayers { get; private set; }

        public int TeamScore { get; private set; }

        public int OpponentScore { get; private set; }

        public int BitsMatchId { get; private set; }
    }
}