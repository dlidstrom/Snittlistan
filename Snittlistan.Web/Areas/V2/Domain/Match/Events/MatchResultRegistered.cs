#nullable enable

namespace Snittlistan.Web.Areas.V2.Domain.Match.Events
{
    using System;
    using System.Collections.Generic;
    using EventStoreLite;

    public class MatchResultRegistered : Event
    {
        public MatchResultRegistered(
            string rosterId,
            List<string> rosterPlayers,
            int teamScore,
            int opponentScore,
            int bitsMatchId,
            string[]? previousPlayerIds = null)
        {
            RosterId = rosterId ?? throw new ArgumentNullException(nameof(rosterId));
            RosterPlayers = rosterPlayers ?? throw new ArgumentNullException(nameof(rosterPlayers));
            TeamScore = teamScore;
            OpponentScore = opponentScore;
            BitsMatchId = bitsMatchId;
            PreviousPlayerIds = previousPlayerIds ?? new string[0];
        }

        public string RosterId { get; private set; }

        public List<string> RosterPlayers { get; private set; }

        public int TeamScore { get; private set; }

        public int OpponentScore { get; private set; }

        public int BitsMatchId { get; private set; }

        public string[] PreviousPlayerIds { get; private set; }
    }
}
