using System;
using System.Collections.Generic;
using System.Linq;
using EventStoreLite;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Raven.Abstractions;
using Snittlistan.Queue.Messages;
using Snittlistan.Web.Areas.V2.Domain.Match.Events;

namespace Snittlistan.Web.Areas.V2.Domain.Match
{
    using Commentary;

    public class MatchResult4 : AggregateRoot
    {
        private readonly Dictionary<string, List<PinsAndScoreResult>> playerPins;
        private HashSet<string> rosterPlayers;
        private bool medalsAwarded;

        // 1-based
        private int registeredSeries;

        public MatchResult4(Roster roster, int teamScore, int opponentScore, int bitsMatchId)
            : this()
        {
            if (roster == null) throw new ArgumentNullException(nameof(roster));
            if (roster.MatchResultId != null)
                throw new ApplicationException("Roster already has result registered");
            VerifyScores(teamScore, opponentScore);

            ApplyChange(
                new MatchResult4Registered(roster.Id, roster.Players, teamScore, opponentScore, bitsMatchId));
        }

        private MatchResult4()
        {
            playerPins = new Dictionary<string, List<PinsAndScoreResult>>();
        }

        private string RosterId { get; set; }

        private int BitsMatchId { get; set; }

        private int OpponentScore { get; set; }

        private int TeamScore { get; set; }

        public bool Update(
            Action<object> publish,
            Roster roster,
            int teamScore,
            int opponentScore,
            int bitsMatchId,
            MatchSerie4[] matchSeries,
            Player[] players)
        {
            // check if anything has changed
            var potentiallyNewPlayerPins = new SortedDictionary<string, List<PinsAndScoreResult>>();
            foreach (var matchSerie in matchSeries)
            {
                foreach (var matchGame in new[] { matchSerie.Game1, matchSerie.Game2, matchSerie.Game3, matchSerie.Game4 })
                {
                    if (potentiallyNewPlayerPins.TryGetValue(matchGame.Player, out var list) == false)
                    {
                        list = new List<PinsAndScoreResult>();
                        potentiallyNewPlayerPins.Add(matchGame.Player, list);
                    }

                    list.Add(new PinsAndScoreResult(matchGame.Pins, matchGame.Score, matchSerie.SerieNumber));
                }
            }

            var oldResult = JsonConvert.SerializeObject(
                new SortedDictionary<string, List<PinsAndScoreResult>>(playerPins),
                Formatting.Indented);
            var newResult = JsonConvert.SerializeObject(
                potentiallyNewPlayerPins,
                Formatting.Indented);
            var pinsOrPlayersDiffer = oldResult != newResult;
            var scoresDiffer = (teamScore, opponentScore).CompareTo((TeamScore, OpponentScore)) != 0;
            if (pinsOrPlayersDiffer || scoresDiffer)
            {
                var @event = new MatchResult4Registered(
                    roster.Id,
                    roster.Players,
                    teamScore,
                    opponentScore,
                    bitsMatchId,
                    playerPins.Keys.AsEnumerable().ToArray());
                ApplyChange(@event);
                RegisterSeries(publish, matchSeries, players, null);
            }

            return roster.Date.AddDays(5) < SystemTime.UtcNow;
        }

        public void RegisterSerie(MatchSerie4 matchSerie)
        {
            if (matchSerie == null) throw new ArgumentNullException(nameof(matchSerie));
            if (rosterPlayers.Count != 4 && rosterPlayers.Count != 5)
                throw new MatchException("Roster must have 4 or 5 players when registering results");
            VerifyPlayers(matchSerie);

            ApplyChange(new Serie4Registered(matchSerie, BitsMatchId, RosterId));
            DoAwardMedals(registeredSeries);
        }

        public void RegisterSeries(
            Action<object> publish,
            MatchSerie4[] matchSeries,
            Player[] players,
            string summaryText)
        {
            if (rosterPlayers.Count != 4 && rosterPlayers.Count != 5)
                throw new MatchException("Roster must have 4 or 5 players when registering results");
            foreach (var matchSerie in matchSeries)
            {
                VerifyPlayers(matchSerie);

                ApplyChange(new Serie4Registered(matchSerie, BitsMatchId, RosterId));
                DoAwardMedals(registeredSeries);
            }

            var analyzer = new Match4Analyzer(matchSeries, players.ToDictionary(x => x.Id));
            var bodyText = analyzer.GetBodyText();
            if (string.IsNullOrWhiteSpace(summaryText))
            {
                ApplyChange(new MatchCommentaryEvent(BitsMatchId, RosterId, bodyText, new string[0]));
            }
            else
            {
                ApplyChange(new MatchCommentaryEvent(BitsMatchId, RosterId, summaryText, new[] { bodyText }));
            }

            publish.Invoke(new MatchRegisteredEvent(RosterId, TeamScore, OpponentScore));
        }

        public void AwardMedals()
        {
            if (medalsAwarded)
                throw new ApplicationException("Medals have already been awarded");
            for (var i = 1; i <= registeredSeries; i++)
            {
                DoAwardMedals(i);
            }

            ApplyChange(new MedalsAwarded());
        }

        public void ClearMedals()
        {
            ApplyChange(new ClearMedals(BitsMatchId, RosterId));
        }

        private static void VerifyScores(int teamScore, int opponentScore)
        {
            if (teamScore < 0 || teamScore > 20)
            {
                throw new ArgumentOutOfRangeException(nameof(teamScore), "Team score must be between 0 and 20");
            }

            if (opponentScore < 0 || opponentScore > 20)
            {
                throw new ArgumentOutOfRangeException(nameof(opponentScore), "Opponent score must be between 0 and 20");
            }

            if (teamScore + opponentScore > 20)
            {
                throw new ArgumentException("Team score and opponent score must be at most 20");
            }
        }

        private void DoAwardMedals(int serie)
        {
            foreach (var key in playerPins.Keys)
            {
                var pinsResult = playerPins[key].SingleOrDefault(x => x.SerieNumber == serie);

                if (pinsResult?.Pins >= 270)
                {
                    var medal = new AwardedMedal(
                        BitsMatchId,
                        RosterId,
                        key,
                        MedalType.PinsInSerie,
                        pinsResult.Pins);
                    ApplyChange(medal);
                }
            }

            if (serie == 4)
            {
                foreach (var key in playerPins.Keys)
                {
                    var list = playerPins[key];
                    var score = list.Sum(x => x.Score);
                    if (score != 4) continue;
                    var medal = new AwardedMedal(
                        BitsMatchId,
                        RosterId,
                        key,
                        MedalType.TotalScore,
                        4);
                    ApplyChange(medal);
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
        private void Apply(Serie4Registered e)
        {
            registeredSeries++;
            foreach (var game in new[] { e.MatchSerie.Game1, e.MatchSerie.Game2, e.MatchSerie.Game3, e.MatchSerie.Game4 })
            {
                if (playerPins.ContainsKey(game.Player) == false)
                {
                    playerPins.Add(game.Player, new List<PinsAndScoreResult>());
                }

                var pinsAndScoreResult = new PinsAndScoreResult(
                    game.Pins,
                    game.Score,
                    registeredSeries);
                playerPins[game.Player].Add(pinsAndScoreResult);
            }
        }

        [UsedImplicitly]
        private void Apply(MedalsAwarded e)
        {
            medalsAwarded = true;
        }

        [UsedImplicitly]
        private void Apply(ClearMedals e)
        {
            medalsAwarded = false;
        }

        [UsedImplicitly]
        private void Apply(AwardedMedal e)
        {
        }

        [UsedImplicitly]
        private void Apply(MatchCommentaryEvent e)
        {
        }
    }
}