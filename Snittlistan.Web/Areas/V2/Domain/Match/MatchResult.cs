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

            ApplyChange(
                new MatchResultRegistered(roster.Id, roster.Players, teamScore, opponentScore, bitsMatchId));
        }

        public void Update(Roster roster, int teamScore, int opponentScore, int bitsMatchId)
        {
            if (roster == null) throw new ArgumentNullException("roster");
            VerifyScores(teamScore, opponentScore);

            roster.MatchResultId = Id;

            if (roster.Id != RosterId)
                ApplyChange(new RosterChanged(RosterId, roster.Id));
            var matchResultUpdated = new MatchResultUpdated(roster.Id, roster.Players, teamScore, opponentScore, bitsMatchId, RosterId, TeamScore, OpponentScore, BitsMatchId);
            ApplyChange(matchResultUpdated);
        }

        public void Delete()
        {
            ApplyChange(new MatchResultDeleted(RosterId, BitsMatchId));
        }

        public void RegisterSerie(MatchSerie matchSerie)
        {
            if (matchSerie == null) throw new ArgumentNullException("matchSerie");
            if (rosterPlayers.Count != 8 && rosterPlayers.Count != 9)
                throw new MatchException("Roster must have 8 or 9 players when registering results");
            VerifyPlayers(matchSerie);

            ApplyChange(new SerieRegistered(matchSerie, BitsMatchId));
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
            RosterId = e.RosterId;
            TeamScore = e.TeamScore;
            OpponentScore = e.OpponentScore;
            BitsMatchId = e.BitsMatchId;
            rosterPlayers = new HashSet<string>(e.RosterPlayers);
        }

        [UsedImplicitly]
        private void Apply(MatchResultUpdated e)
        {
            RosterId = e.NewRosterId;
            TeamScore = e.NewTeamScore;
            OpponentScore = e.NewOpponentScore;
            BitsMatchId = e.NewBitsMatchId;
            rosterPlayers = new HashSet<string>(e.RosterPlayers);
        }
    }
}