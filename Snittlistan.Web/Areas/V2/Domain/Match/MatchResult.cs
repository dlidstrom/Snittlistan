using System;
using EventStoreLite;
using Snittlistan.Web.Areas.V2.Domain.Match.Events;

namespace Snittlistan.Web.Areas.V2.Domain.Match
{
    public class MatchResult : AggregateRoot
    {
        public MatchResult(Roster roster, int teamScore, int opponentScore, int bitsMatchId)
        {
            if (roster == null) throw new ArgumentNullException("roster");
            if (roster.MatchResultId != null)
                throw new ApplicationException("Roster already has result registered");
            if (teamScore < 0 || teamScore > 20)
                throw new ArgumentException("Team score must be between 0 and 20", "teamScore");
            if (opponentScore < 0 || opponentScore > 20)
                throw new ArgumentException("Opponent score must be between 0 and 20", "opponentScore");

            this.ApplyChange(new MatchResultRegistered(roster.Id, teamScore, opponentScore, bitsMatchId));
        }

        public void Update(Roster roster, int teamScore, int opponentScore, int bitsMatchId)
        {
            if (roster == null) throw new ArgumentNullException("roster");
            if (teamScore < 0 || teamScore > 20)
                throw new ArgumentException("Team score must be between 0 and 20", "teamScore");
            if (opponentScore < 0 || opponentScore > 20)
                throw new ArgumentException("Opponent score must be between 0 and 20", "opponentScore");

            if (roster.Id != this.RosterId)
                this.ApplyChange(new RosterChanged(this.RosterId, roster.Id));
            var matchResultUpdated = new MatchResultUpdated(roster.Id, teamScore, opponentScore, bitsMatchId, RosterId, TeamScore, OpponentScore, BitsMatchId);
            this.ApplyChange(matchResultUpdated);
        }

        public void Delete()
        {
            this.ApplyChange(new MatchResultDeleted(RosterId, BitsMatchId));
        }

        public void RegisterSerie(MatchSerie matchSerie)
        {
            if (matchSerie == null) throw new ArgumentNullException("matchSerie");
            this.ApplyChange(new SerieRegistered(matchSerie));
        }

        private string RosterId { get; set; }

        private int BitsMatchId { get; set; }

        private int OpponentScore { get; set; }

        private int TeamScore { get; set; }

        // events
        private void Apply(MatchResultRegistered e)
        {
            this.RosterId = e.RosterId;
            this.TeamScore = e.TeamScore;
            this.OpponentScore = e.OpponentScore;
            this.BitsMatchId = e.BitsMatchId;
        }

        private void Apply(MatchResultUpdated e)
        {
            this.RosterId = e.NewRosterId;
        }
    }
}