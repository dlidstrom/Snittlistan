using System;
using System.Collections.Generic;
using EventStoreLite;
using JetBrains.Annotations;
using Snittlistan.Web.Areas.V2.Domain.Match.Events;

namespace Snittlistan.Web.Areas.V2.Domain.Match
{
    public class MatchResult : AggregateRoot
    {
        private HashSet<string> rosterPlayers;

        public MatchResult(Roster roster, int teamScore, int opponentScore, int bitsMatchId)
        {
            if (roster == null) throw new ArgumentNullException("roster");
            if (roster.MatchResultId != null)
                throw new ApplicationException("Roster already has result registered");
            VerifyScores(teamScore, opponentScore);

            this.ApplyChange(
                new MatchResultRegistered(roster.Id, roster.Players, teamScore, opponentScore, bitsMatchId));
        }

        public void Update(Roster roster, int teamScore, int opponentScore, int bitsMatchId)
        {
            if (roster == null) throw new ArgumentNullException("roster");
            VerifyScores(teamScore, opponentScore);

            roster.MatchResultId = Id;

            if (roster.Id != this.RosterId)
                this.ApplyChange(new RosterChanged(this.RosterId, roster.Id));
            var matchResultUpdated = new MatchResultUpdated(roster.Id, roster.Players, teamScore, opponentScore, bitsMatchId, RosterId, TeamScore, OpponentScore, BitsMatchId);
            this.ApplyChange(matchResultUpdated);
        }

        public void Delete()
        {
            this.ApplyChange(new MatchResultDeleted(RosterId, BitsMatchId));
        }

        public void RegisterSerie(MatchSerie matchSerie)
        {
            if (matchSerie == null) throw new ArgumentNullException("matchSerie");
            if (rosterPlayers.Count != 8)
                throw new MatchException("Roster must have 8 players when registering results");
            this.VerifyPlayers(matchSerie);

            this.ApplyChange(new SerieRegistered(matchSerie));
        }

        private string RosterId { get; set; }

        private int BitsMatchId { get; set; }

        private int OpponentScore { get; set; }

        private int TeamScore { get; set; }

        private static void VerifyScores(int teamScore, int opponentScore)
        {
            if (teamScore < 0 || teamScore > 20)
            {
                throw new ArgumentOutOfRangeException("teamScore", "Team score must be between 0 and 20");
            }
            if (opponentScore < 0 || opponentScore > 20)
            {
                throw new ArgumentOutOfRangeException("opponentScore", "Opponent score must be between 0 and 20");
            }
            if (teamScore + opponentScore > 20)
            {
                throw new ArgumentException("Team score and opponent score must be at most 20");
            }
        }

        private void VerifyPlayers(MatchSerie matchSerie)
        {
            do
            {
                if (rosterPlayers.Contains(matchSerie.Table1.Game1.Player) == false) break;
                if (rosterPlayers.Contains(matchSerie.Table1.Game2.Player) == false) break;
                if (rosterPlayers.Contains(matchSerie.Table2.Game1.Player) == false) break;
                if (rosterPlayers.Contains(matchSerie.Table2.Game2.Player) == false) break;
                if (rosterPlayers.Contains(matchSerie.Table3.Game1.Player) == false) break;
                if (rosterPlayers.Contains(matchSerie.Table3.Game2.Player) == false) break;
                if (rosterPlayers.Contains(matchSerie.Table4.Game1.Player) == false) break;
                if (rosterPlayers.Contains(matchSerie.Table4.Game2.Player) == false) break;

                return;
            }
            // ReSharper disable CSharpWarnings::CS0162
            while (false);
            // ReSharper restore CSharpWarnings::CS0162

            throw new MatchException("Can only register players from roster");
        }

        // events
        [UsedImplicitly]
        private void Apply(MatchResultRegistered e)
        {
            this.RosterId = e.RosterId;
            this.TeamScore = e.TeamScore;
            this.OpponentScore = e.OpponentScore;
            this.BitsMatchId = e.BitsMatchId;
            this.rosterPlayers = new HashSet<string>(e.RosterPlayers);
        }

        [UsedImplicitly]
        private void Apply(MatchResultUpdated e)
        {
            this.RosterId = e.NewRosterId;
            this.TeamScore = e.NewTeamScore;
            this.OpponentScore = e.NewOpponentScore;
            this.BitsMatchId = e.NewBitsMatchId;
            this.rosterPlayers = new HashSet<string>(e.RosterPlayers);
        }
    }
}