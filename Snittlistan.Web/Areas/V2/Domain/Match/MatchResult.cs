#nullable enable

namespace Snittlistan.Web.Areas.V2.Domain.Match
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EventStoreLite;
    using Newtonsoft.Json;
    using Raven.Abstractions;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Web.Areas.V2.Domain.Match.Commentary;
    using Snittlistan.Web.Areas.V2.Domain.Match.Events;
    using Snittlistan.Web.Areas.V2.Indexes;
    using Snittlistan.Web.Areas.V2.ReadModels;

    public class MatchResult : AggregateRoot
    {
        private Dictionary<string, List<PinsAndScoreResult>> playerPins;
        private HashSet<string>? rosterPlayers;
        private bool medalsAwarded;
        private string matchCommentaryAsJson = string.Empty;

        // 1-based
        private int registeredSeries;

        public MatchResult(Roster roster, int teamScore, int opponentScore, int bitsMatchId)
            : this()
        {
            if (roster == null)
            {
                throw new ArgumentNullException(nameof(roster));
            }

            if (roster.MatchResultId != null)
            {
                throw new ApplicationException("Roster already has result registered");
            }

            VerifyScores(teamScore, opponentScore);

            ApplyChange(
                new MatchResultRegistered(roster.Id!, roster.Players, teamScore, opponentScore, bitsMatchId));
        }

        private MatchResult()
        {
            playerPins = new Dictionary<string, List<PinsAndScoreResult>>();
        }

        private string? RosterId { get; set; }

        private int BitsMatchId { get; set; }

        private int OpponentScore { get; set; }

        private int TeamScore { get; set; }

        public bool Update(
            Action<ITask> publish,
            Roster roster,
            int teamScore,
            int opponentScore,
            MatchSerie[] matchSeries,
            ResultSeriesReadModel.Serie[] opponentSeries,
            Player[] players,
            Dictionary<string, ResultForPlayerIndex.Result> resultsForPlayer)
        {
            // check if anything has changed
            SortedDictionary<string, List<PinsAndScoreResult>> potentiallyNewPlayerPins = new();
            foreach (MatchSerie matchSerie in matchSeries)
            {
                foreach (MatchTable matchTable in new[] { matchSerie.Table1, matchSerie.Table2, matchSerie.Table3, matchSerie.Table4 })
                {
                    foreach (MatchGame game in new[] { matchTable.Game1, matchTable.Game2 })
                    {
                        if (potentiallyNewPlayerPins.TryGetValue(game.Player, out List<PinsAndScoreResult> list) == false)
                        {
                            list = new List<PinsAndScoreResult>();
                            potentiallyNewPlayerPins.Add(game.Player, list);
                        }

                        list.Add(new PinsAndScoreResult(game.Pins, matchTable.Score, matchSerie.SerieNumber));
                    }
                }
            }

            string oldResult = JsonConvert.SerializeObject(
                new SortedDictionary<string, List<PinsAndScoreResult>>(playerPins),
                Formatting.Indented);
            string newResult = JsonConvert.SerializeObject(
                potentiallyNewPlayerPins,
                Formatting.Indented);
            bool pinsOrPlayersDiffer = oldResult != newResult;
            bool scoresDiffer = (teamScore, opponentScore).CompareTo((TeamScore, OpponentScore)) != 0;
            MatchCommentaryEvent matchCommentaryEvent = CreateMatchCommentary(
                matchSeries,
                opponentSeries,
                players.ToDictionary(x => x.Id),
                resultsForPlayer);
            string newMatchCommentaryAsJson = JsonConvert.SerializeObject(
                new { matchCommentaryEvent.BodyText, matchCommentaryEvent.SummaryText },
                Formatting.Indented);
            bool commentaryDiffers = matchCommentaryAsJson != newMatchCommentaryAsJson;

            if (pinsOrPlayersDiffer || scoresDiffer || commentaryDiffers)
            {
                MatchResultRegistered @event = new(
                    roster.Id!,
                    roster.Players,
                    teamScore,
                    opponentScore,
                    roster.BitsMatchId,
                    playerPins.Keys.AsEnumerable().ToArray());
                ApplyChange(@event);
                RegisterSeries(publish, matchSeries, opponentSeries, players, resultsForPlayer);
            }

            return roster.Date.AddDays(5) < SystemTime.UtcNow;
        }

        public void RegisterSeries(
            Action<ITask> publish,
            MatchSerie[] matchSeries,
            ResultSeriesReadModel.Serie[] opponentSeries,
            Player[] players,
            Dictionary<string, ResultForPlayerIndex.Result> resultsForPlayer)
        {
            if (matchSeries == null)
            {
                throw new ArgumentNullException(nameof(matchSeries));
            }

            if (opponentSeries == null)
            {
                throw new ArgumentNullException(nameof(opponentSeries));
            }

            if (players == null)
            {
                throw new ArgumentNullException(nameof(players));
            }

            if (resultsForPlayer == null)
            {
                throw new ArgumentNullException(nameof(resultsForPlayer));
            }

            if (rosterPlayers!.Count is not 8 and not 9 and not 10)
            {
                throw new MatchException("Roster must have 8, 9, or 10 players when registering results");
            }

            foreach (MatchSerie matchSerie in matchSeries)
            {
                VerifyPlayers(matchSerie);
                ApplyChange(new SerieRegistered(matchSerie, BitsMatchId, RosterId!));
                DoAwardMedals(registeredSeries);
                if (registeredSeries > 4)
                {
                    throw new ArgumentException("Can only register up to 4 series");
                }
            }

            MatchCommentaryEvent matchCommentaryEvent = CreateMatchCommentary(
                matchSeries,
                opponentSeries,
                players.ToDictionary(x => x.Id),
                resultsForPlayer);
            ApplyChange(matchCommentaryEvent);
            publish.Invoke(new MatchRegisteredTask(RosterId!, BitsMatchId, TeamScore, OpponentScore));
        }

        public void RegisterSerie(MatchTable[] matchTables)
        {
            if (matchTables == null)
            {
                throw new ArgumentNullException(nameof(matchTables));
            }

            if (rosterPlayers!.Count is not 8 and not 9 and not 10)
            {
                throw new MatchException("Roster must have 8, 9, or 10 players when registering results");
            }

            MatchSerie matchSerie = new(registeredSeries + 1, matchTables);
            VerifyPlayers(matchSerie);

            ApplyChange(new SerieRegistered(matchSerie, BitsMatchId, RosterId!));
            DoAwardMedals(registeredSeries);
        }

        public void AwardMedals()
        {
            if (medalsAwarded)
            {
                throw new ApplicationException("Medals have already been awarded");
            }

            for (int i = 1; i <= registeredSeries; i++)
            {
                DoAwardMedals(i);
            }

            ApplyChange(new MedalsAwarded());
        }

        public void ClearMedals()
        {
            ApplyChange(new ClearMedals(BitsMatchId, RosterId!));
        }

        public void AwardScores()
        {
            Dictionary<string, int> dict = new();
            foreach (string key in playerPins.Keys)
            {
                int totalScore = playerPins[key].Sum(x => x.Score);
                dict[key] = totalScore;
            }

            ApplyChange(new ScoreAwarded(dict, BitsMatchId, RosterId!));
        }

        private static void VerifyScores(int teamScore, int opponentScore)
        {
            if (teamScore is < 0 or > 20)
            {
                throw new ArgumentOutOfRangeException(nameof(teamScore), "Team score must be between 0 and 20");
            }

            if (opponentScore is < 0 or > 20)
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
            foreach (string key in playerPins.Keys)
            {
                List<PinsAndScoreResult> list = playerPins[key];
                foreach (PinsAndScoreResult pinsResult in list.Where(x => x.SerieNumber == serieNumber))
                {
                    if (pinsResult.Pins < 270)
                    {
                        continue;
                    }

                    AwardedMedal medal = new(
                        BitsMatchId,
                        RosterId!,
                        key,
                        MedalType.PinsInSerie,
                        pinsResult.Pins);
                    ApplyChange(medal);
                }
            }

            if (serieNumber == 4)
            {
                foreach (string key in playerPins.Keys)
                {
                    List<PinsAndScoreResult> list = playerPins[key];
                    int score = list.Sum(x => x.Score);
                    if (score != 4)
                    {
                        continue;
                    }

                    AwardedMedal medal = new(
                        BitsMatchId,
                        RosterId!,
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
                if (rosterPlayers!.Contains(matchSerie.Table1.Game1.Player) == false)
                {
                    break;
                }

                if (rosterPlayers.Contains(matchSerie.Table1.Game2.Player) == false)
                {
                    break;
                }

                if (rosterPlayers.Contains(matchSerie.Table2.Game1.Player) == false)
                {
                    break;
                }

                if (rosterPlayers.Contains(matchSerie.Table2.Game2.Player) == false)
                {
                    break;
                }

                if (rosterPlayers.Contains(matchSerie.Table3.Game1.Player) == false)
                {
                    break;
                }

                if (rosterPlayers.Contains(matchSerie.Table3.Game2.Player) == false)
                {
                    break;
                }

                if (rosterPlayers.Contains(matchSerie.Table4.Game1.Player) == false)
                {
                    break;
                }

                if (rosterPlayers.Contains(matchSerie.Table4.Game2.Player) == false)
                {
                    break;
                }

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
            MatchAnalyzer commentaryAnalyzer = new(matchSeries, opponentSeries, players);
            string summaryText = commentaryAnalyzer.GetSummaryText();
            string[] bodyText = commentaryAnalyzer.GetBodyText(resultsForPlayer);
            return new MatchCommentaryEvent(BitsMatchId, RosterId!, summaryText, summaryText, bodyText);
        }

        // events
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

        private void Apply(SerieRegistered e)
        {
            registeredSeries++;
            foreach (MatchTable table in new[] { e.MatchSerie.Table1, e.MatchSerie.Table2, e.MatchSerie.Table3, e.MatchSerie.Table4 })
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

        private void Apply(MedalsAwarded _)
        {
            medalsAwarded = true;
        }

        private void Apply(ClearMedals _)
        {
            medalsAwarded = false;
        }

        private void Apply(ScoreAwarded _)
        {
        }

        private void Apply(MatchCommentaryEvent e)
        {
            matchCommentaryAsJson = JsonConvert.SerializeObject(
                new { e.BodyText, e.SummaryText },
                Formatting.Indented);
        }

        private void Apply(AwardedMedal _)
        {
        }
    }
}
