using System;
using System.Collections.Generic;
using EventStoreLite;
using JetBrains.Annotations;
using Snittlistan.Web.Areas.V2.Domain.Match.Events;

namespace Snittlistan.Web.Areas.V2.Domain.Match
{
    public class MatchResult4 : AggregateRoot
    {
        private readonly Dictionary<string, int> playerScores;
        private readonly Dictionary<string, List<int>> playerPins;
        private HashSet<string> rosterPlayers;
        private bool medalsAwarded;

        public MatchResult4(Roster roster, int teamScore, int opponentScore, int bitsMatchId)
            : this()
        {
            if (roster == null) throw new ArgumentNullException("roster");
            if (roster.MatchResultId != null)
                throw new ApplicationException("Roster already has result registered");
            VerifyScores(teamScore, opponentScore);

            ApplyChange(
                new MatchResult4Registered(roster.Id, roster.Players, teamScore, opponentScore, bitsMatchId));
        }

        private MatchResult4()
        {
            playerScores = new Dictionary<string, int>();
            playerPins = new Dictionary<string, List<int>>();
        }

        private string RosterId { get; set; }

        private int BitsMatchId { get; set; }

        private int OpponentScore { get; set; }

        private int TeamScore { get; set; }

        public void Update(Roster roster, int teamScore, int opponentScore, int bitsMatchId)
        {
            if (roster == null) throw new ArgumentNullException("roster");
            VerifyScores(teamScore, opponentScore);

            roster.MatchResultId = Id;

            if (roster.Id != RosterId)
                ApplyChange(new Roster4Changed(RosterId, roster.Id));
            var matchResultUpdated = new MatchResult4Updated(roster.Id, roster.Players, teamScore, opponentScore, bitsMatchId, RosterId, TeamScore, OpponentScore, BitsMatchId);
            ApplyChange(matchResultUpdated);
        }

        public void Delete()
        {
            ApplyChange(new MatchResult4Deleted(RosterId, BitsMatchId));
        }

        public void RegisterSerie(MatchSerie4 matchSerie)
        {
            if (matchSerie == null) throw new ArgumentNullException("matchSerie");
            if (rosterPlayers.Count != 4 && rosterPlayers.Count != 5)
                throw new MatchException("Roster must have 4 or 5 players when registering results");
            VerifyPlayers(matchSerie);

            ApplyChange(new Serie4Registered(matchSerie, BitsMatchId));
            DoAwardMedals();
        }

        public void AwardMedals()
        {
            if (medalsAwarded)
                throw new ApplicationException("Medals have already been awarded");
            DoAwardMedals();
            ApplyChange(new MedalsAwarded());
        }

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

        private void DoAwardMedals()
        {
            foreach (var key in playerScores.Keys)
            {
                var value = playerScores[key];
                if (value == 4)
                    ApplyChange(new AwardedMedal(BitsMatchId, key, MedalType.TotalScore, 4));
            }

            foreach (var key in playerPins.Keys)
            {
                foreach (var pins in playerPins[key])
                {
                    if (pins >= 270)
                        ApplyChange(new AwardedMedal(BitsMatchId, key, MedalType.PinsInSerie, pins));
                }
            }
        }

        private void VerifyPlayers(MatchSerie4 matchSerie)
        {
            do
            {
                if (rosterPlayers.Contains(matchSerie.Game1.Player) == false) break;
                if (rosterPlayers.Contains(matchSerie.Game2.Player) == false) break;
                if (rosterPlayers.Contains(matchSerie.Game3.Player) == false) break;
                if (rosterPlayers.Contains(matchSerie.Game4.Player) == false) break;

                return;
            }
            while (false);

            throw new MatchException("Can only register players from roster");
        }

        // events
        [UsedImplicitly]
        private void Apply(MatchResult4Registered e)
        {
            RosterId = e.RosterId;
            TeamScore = e.TeamScore;
            OpponentScore = e.OpponentScore;
            BitsMatchId = e.BitsMatchId;
            rosterPlayers = new HashSet<string>(e.RosterPlayers);
        }

        [UsedImplicitly]
        private void Apply(MatchResult4Updated e)
        {
            RosterId = e.NewRosterId;
            TeamScore = e.NewTeamScore;
            OpponentScore = e.NewOpponentScore;
            BitsMatchId = e.NewBitsMatchId;
            rosterPlayers = new HashSet<string>(e.RosterPlayers);
        }

        [UsedImplicitly]
        private void Apply(Serie4Registered e)
        {
            foreach (var game in new[] { e.MatchSerie.Game1, e.MatchSerie.Game2, e.MatchSerie.Game3, e.MatchSerie.Game4 })
            {
                if (playerScores.ContainsKey(game.Player))
                {
                    playerScores[game.Player] += game.Score;
                }
                else
                {
                    playerScores[game.Player] = game.Score;
                }

                if (playerPins.ContainsKey(game.Player) == false)
                {
                    playerPins.Add(game.Player, new List<int>());
                }

                playerPins[game.Player].Add(game.Pins);
            }
        }

        [UsedImplicitly]
        private void Apply(MedalsAwarded e)
        {
            medalsAwarded = true;
        }
    }
}