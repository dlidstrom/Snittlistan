using System;
using System.Collections.Generic;
using EventStoreLite;

namespace Snittlistan.Web.Areas.V2.Domain.Match.Events
{
    public class MatchResultRegistered : Event
    {
        public string RosterId { get; set; }

        public List<string> RosterPlayers { get; set; }

        public int TeamScore { get; set; }

        public int OpponentScore { get; set; }

        public int BitsMatchId { get; set; }

        public MatchResultRegistered(
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

            this.RosterId = rosterId;
            this.RosterPlayers = rosterPlayers;
            this.TeamScore = teamScore;
            this.OpponentScore = opponentScore;
            this.BitsMatchId = bitsMatchId;
        }
    }
}