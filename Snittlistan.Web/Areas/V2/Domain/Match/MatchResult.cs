using System;
using System.Collections.Generic;
using System.Linq;
using EventStoreLite;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Raven.Abstractions;
using Snittlistan.Web.Areas.V2.Domain.Match.Commentary;
using Snittlistan.Web.Areas.V2.Domain.Match.Events;
using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Web.Areas.V2.ReadModels;
using Snittlistan.Web.DomainEvents;

namespace Snittlistan.Web.Areas.V2.Domain.Match
{
    public class MatchResult : AggregateRoot
    {
        private Dictionary<string, List<PinsAndScoreResult>> playerPins;
        private HashSet<string> rosterPlayers;
        private bool medalsAwarded;

        // 1-based
        private int registeredSeries;

        public MatchResult(Roster roster, int teamScore, int opponentScore, int bitsMatchId)
            : this()
        {
            if (roster == null) throw new ArgumentNullException(nameof(roster));
            if (roster.MatchResultId != null)
                throw new ApplicationException("Roster already has result registered");
            VerifyScores(teamScore, opponentScore);

            ApplyChange(
                new MatchResultRegistered(roster.Id, roster.Players, teamScore, opponentScore, bitsMatchId));
        }

        private MatchResult()
        {
            playerPins = new Dictionary<string, List<PinsAndScoreResult>>();
        }

        private string RosterId { get; set; }

        private int BitsMatchId { get; set; }

        private int OpponentScore { get; set; }

        private int TeamScore { get; set; }

        public void Update(Roster roster, int teamScore, int opponentScore, int bitsMatchId)
        {
            if (roster == null) throw new ArgumentNullException(nameof(roster));
            VerifyScores(teamScore, opponentScore);

            roster.MatchResultId = Id;

            if (roster.Id != RosterId)
                ApplyChange(new RosterChanged(RosterId, roster.Id));
            var matchResultUpdated = new MatchResultUpdated(roster.Id, roster.Players, teamScore, opponentScore, bitsMatchId, RosterId, TeamScore, OpponentScore, BitsMatchId);
            ApplyChange(matchResultUpdated);
        }

        public bool Update(
            Roster roster,
            int teamScore,
            int opponentScore,
            MatchSerie[] matchSeries,
            ResultSeriesReadModel.Serie[] opponentSeries,
            Player[] players,
            Dictionary<string, ResultForPlayerIndex.Result> resultsForPlayer)
        {
            // check if anything has changed
            var potentiallyNewPlayerPins = new SortedDictionary<string, List<PinsAndScoreResult>>();
            foreach (var matchSerie in matchSeries)
            {
                foreach (var matchTable in new[] { matchSerie.Table1, matchSerie.Table2, matchSerie.Table3, matchSerie.Table4 })
                {
                    foreach (var game in new[] { matchTable.Game1, matchTable.Game2 })
                    {
                        if (potentiallyNewPlayerPins.TryGetValue(game.Player, out var list) == false)
                        {
                            list = new List<PinsAndScoreResult>();
                            potentiallyNewPlayerPins.Add(game.Player, list);
                        }

                        list.Add(new PinsAndScoreResult(game.Pins, matchTable.Score, matchSerie.SerieNumber));
                    }
                }
            }

            var oldResult = JsonConvert.SerializeObject(
                new SortedDictionary<string, List<PinsAndScoreResult>>(playerPins),
                Formatting.Indented);
            var newResult = JsonConvert.SerializeObject(potentiallyNewPlayerPins, Formatting.Indented);
            var pinsOrPlayersDiffer = oldResult != newResult;
            var scoresDiffer = (teamScore, opponentScore).CompareTo((TeamScore, OpponentScore)) != 0;
            if (pinsOrPlayersDiffer || scoresDiffer)
            {
                ApplyChange(new MatchResultRegistered(roster.Id, roster.Players, teamScore, opponentScore, roster.BitsMatchId));
                RegisterSeries(matchSeries, opponentSeries, players, resultsForPlayer);
            }

            return roster.Date.AddDays(5) < SystemTime.UtcNow;
        }

        public void RegisterSeries(
            MatchSerie[] matchSeries,
            ResultSeriesReadModel.Serie[] opponentSeries,
            Player[] players,
            Dictionary<string, ResultForPlayerIndex.Result> resultsForPlayer)
        {
            if (matchSeries == null) throw new ArgumentNullException(nameof(matchSeries));
            if (opponentSeries == null) throw new ArgumentNullException(nameof(opponentSeries));
            if (players == null) throw new ArgumentNullException(nameof(players));
            if (resultsForPlayer == null) throw new ArgumentNullException(nameof(resultsForPlayer));
            if (rosterPlayers.Count != 8 && rosterPlayers.Count != 9 && rosterPlayers.Count != 10)
                throw new MatchException("Roster must have 8, 9, or 10 players when registering results");
            foreach (var matchSerie in matchSeries)
            {
                VerifyPlayers(matchSerie);
                ApplyChange(new SerieRegistered(matchSerie, BitsMatchId, RosterId));
                DoAwardMedals(registeredSeries);
                if (registeredSeries > 4) throw new ArgumentException("Can only register up to 4 series");
            }

            var matchCommentaryEvent = CreateMatchCommentary(
                matchSeries,
                opponentSeries,
                players.ToDictionary(x => x.Id),
                resultsForPlayer);
            ApplyChange(matchCommentaryEvent);
            DomainEvent.Raise(new MatchRegisteredEvent(RosterId, TeamScore, OpponentScore));
        }

        public void RegisterSerie(MatchTable[] matchTables)
        {
            if (matchTables == null) throw new ArgumentNullException(nameof(matchTables));
            if (rosterPlayers.Count != 8 && rosterPlayers.Count != 9 && rosterPlayers.Count != 10)
                throw new MatchException("Roster must have 8, 9, or 10 players when registering results");
            var matchSerie = new MatchSerie(registeredSeries + 1, matchTables);
            VerifyPlayers(matchSerie);

            ApplyChange(new SerieRegistered(matchSerie, BitsMatchId, RosterId));
            DoAwardMedals(registeredSeries);
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
            ApplyChange(new ClearMedals(BitsMatchId));
        }

        public void AwardScores()
        {
            var dict = new Dictionary<string, int>();
            foreach (var key in playerPins.Keys)
            {
                var totalScore = playerPins[key].Sum(x => x.Score);
                dict[key] = totalScore;
            }

            ApplyChange(new ScoreAwarded(dict, BitsMatchId));
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

        private void DoAwardMedals(int serieNumber)
        {
            foreach (var key in playerPins.Keys)
            {
                var list = playerPins[key];
                foreach (var pinsResult in list.Where(x => x.SerieNumber == serieNumber))
                {
                    if (pinsResult.Pins < 270) continue;
                    var medal = new AwardedMedal(
                        BitsMatchId,
                        key,
                        MedalType.PinsInSerie,
                        pinsResult.Pins);
                    ApplyChange(medal);
                }
            }

            if (serieNumber == 4)
            {
                foreach (var key in playerPins.Keys)
                {
                    var list = playerPins[key];
                    var score = list.Sum(x => x.Score);
                    if (score != 4) continue;
                    var medal = new AwardedMedal(
                        BitsMatchId,
                        key,
                        MedalType.TotalScore,
                        4);
                    ApplyChange(medal);
                }
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
            while (false);

            throw new MatchException("Can only register players from roster");
        }

        private MatchCommentaryEvent CreateMatchCommentary(
            MatchSerie[] matchSeries,
            ResultSeriesReadModel.Serie[] opponentSeries,
            Dictionary<string, Player> players,
            Dictionary<string, ResultForPlayerIndex.Result> resultsForPlayer)
        {
            var commentaryAnalyzer = new MatchAnalyzer(matchSeries, opponentSeries, players);
            var summaryText = commentaryAnalyzer.GetSummaryText();
            var bodyText = commentaryAnalyzer.GetBodyText(resultsForPlayer);
            return new MatchCommentaryEvent(BitsMatchId, summaryText, bodyText);
        }

        // events
        [UsedImplicitly]
        private void Apply(MatchResultRegistered e)
        {
            RosterId = e.RosterId;
            TeamScore = e.TeamScore;
            OpponentScore = e.OpponentScore;
            BitsMatchId = e.BitsMatchId;
            playerPins = new Dictionary<string, List<PinsAndScoreResult>>();
            registeredSeries = 0;
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

        [UsedImplicitly]
        private void Apply(SerieRegistered e)
        {
            registeredSeries++;
            foreach (var table in new[] { e.MatchSerie.Table1, e.MatchSerie.Table2, e.MatchSerie.Table3, e.MatchSerie.Table4 })
            {
                if (playerPins.ContainsKey(table.Game1.Player) == false)
                {
                    playerPins.Add(table.Game1.Player, new List<PinsAndScoreResult>());
                }

                playerPins[table.Game1.Player].Add(new PinsAndScoreResult(table.Game1.Pins, table.Score, registeredSeries));

                if (playerPins.ContainsKey(table.Game2.Player) == false)
                {
                    playerPins.Add(table.Game2.Player, new List<PinsAndScoreResult>());
                }

                playerPins[table.Game2.Player].Add(new PinsAndScoreResult(table.Game2.Pins, table.Score, registeredSeries));
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
        private void Apply(ScoreAwarded e)
        {
        }

        [UsedImplicitly]
        private void Apply(MatchCommentaryEvent e)
        {
        }

        [UsedImplicitly]
        private void Apply(AwardedMedal e)
        {
        }
    }
}