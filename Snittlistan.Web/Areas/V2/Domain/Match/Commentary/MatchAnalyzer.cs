#nullable enable

namespace Snittlistan.Web.Areas.V2.Domain.Match.Commentary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Snittlistan.Web.Areas.V2.Indexes;
    using Snittlistan.Web.Areas.V2.ReadModels;

    public class MatchAnalyzer
    {
        private readonly MatchSerie[] matchSeries;
        private readonly ResultSeriesReadModel.Serie[] opponentSeries;
        private readonly Dictionary<string, Player> players;

        public MatchAnalyzer(
            MatchSerie[] matchSeries,
            ResultSeriesReadModel.Serie[] opponentSeries,
            Dictionary<string, Player> players)
        {
            this.matchSeries = matchSeries;
            this.opponentSeries = opponentSeries;
            this.players = players;
        }

        public string GetSummaryText()
        {
            SeriesScores[] seriesScores = GetSeriesScores();
            SummaryPattern[] summaryPatterns = SummaryPatterns.Create(players);

            SummaryPattern[] matches = summaryPatterns.Where(x => x.Matches(seriesScores)).ToArray();
            string matchCommentary;
            if (matches.Length > 1)
            {
                matchCommentary = string.Join(", ", matches.Select(x => $"'{x.Description}'"));
            }
            else if (matches.Length == 0)
            {
                matchCommentary = "No matching pattern";
            }
            else
            {
                matchCommentary = matches[0].Commentary.Invoke(seriesScores);
            }

            return matchCommentary;
        }

        public string[] GetBodyText(
            Dictionary<string, ResultForPlayerIndex.Result> resultsForPlayer)
        {
            string nicknameFormatter(Tuple<string, int>[] ids, bool includeResult)
            {
                string formatOne(string nickname, int pins)
                {
                    return
                        includeResult
                        ? $"{nickname} ({pins})"
                        : nickname;
                }

                if (ids.Length == 1)
                {
                    return formatOne(players[ids.First().Item1].Nickname!, ids.First().Item2);
                }

                string firstNicknames = string.Join(
                    ", ",
                    ids.Take(ids.Length - 1)
                       .Select(x => formatOne(players[x.Item1].Nickname!, x.Item2)));
                string nicknames = $"{firstNicknames} och {formatOne(players[ids.Last().Item1].Nickname!, ids.Last().Item2)}";
                return nicknames;
            }

            List<string> bodyText = new();
            SeriesScores[] seriesScores = GetSeriesScores();
            foreach (SeriesScores seriesScore in seriesScores)
            {
                List<string> seriesText = new();
                bool hasSerieNumber = false;
                HashSet<Tuple<string, int>> playersContributingToWin = new();
                int diff = seriesScore.TeamPins - seriesScore.OpponentPins;
                if (diff <= 20 && seriesScore.MatchResult == MatchResultType.Win)
                {
                    hasSerieNumber = true;
                    seriesText.Add(
                        $"Serie {seriesScore.SerieNumber} vanns med enbart {diff} pinnar.");
                    foreach (PlayerResult playerResult in seriesScore.PlayerResults)
                    {
                        if ((seriesScore.TeamPins - playerResult.Pins) / 7 < playerResult.Pins - 20)
                        {
                            _ = playersContributingToWin.Add(Tuple.Create(playerResult.PlayerId, playerResult.Pins));
                        }
                    }

                    if (playersContributingToWin.Any())
                    {
                        string joiner = playersContributingToWin.Count > 1 ? "sina" : "sitt";
                        string nicknames = nicknameFormatter(playersContributingToWin.OrderByDescending(x => x.Item2).ToArray(), true);
                        seriesText.Add(
                            $"{nicknames} låg bakom serievinsten med {joiner} fina resultat.");
                    }
                }

                HashSet<Tuple<string, int>> greatSeries = new();
                foreach (PlayerResult playerResult in seriesScore.PlayerResults)
                {
                    Tuple<string, int> lookingFor = Tuple.Create(playerResult.PlayerId, playerResult.Pins);
                    if (playerResult.Pins >= 270 && playersContributingToWin.Contains(lookingFor) == false)
                    {
                        _ = greatSeries.Add(lookingFor);
                    }
                }

                if (greatSeries.Any())
                {
                    Tuple<string, int>[] seriesExcept300 = greatSeries.Where(x => x.Item2 != 300).ToArray();
                    Tuple<string, int>[] series300 = greatSeries.Where(x => x.Item2 == 300).ToArray();
                    string trail = seriesExcept300.Length > 1 ? "utmärkta insatser" : "utmärkt insats";
                    if (seriesExcept300.Any())
                    {
                        if (!hasSerieNumber)
                        {
                            string sentence = $"I serie {seriesScore.SerieNumber} stod {nicknameFormatter(seriesExcept300.OrderByDescending(x => x.Item2).ToArray(), true)} för {trail}.";
                            seriesText.Add(sentence);
                        }
                        else
                        {
                            string sentence = $"{nicknameFormatter(seriesExcept300.OrderByDescending(x => x.Item2).ToArray(), true)} stod för bra {trail}.";
                            seriesText.Add(sentence);
                        }
                    }

                    if (series300.Any())
                    {
                        if (!hasSerieNumber)
                        {
                            string sentence = $"I serie {seriesScore.SerieNumber} smällde {nicknameFormatter(series300, false)} till med 300!";
                            seriesText.Add(sentence);
                        }
                        else
                        {
                            string sentence = $"{nicknameFormatter(series300, false)} smällde till med 300!";
                            seriesText.Add(sentence);
                        }
                    }
                }

                if (seriesText.Any())
                {
                    bodyText.Add(string.Join(" ", seriesText));
                }
            }

            List<string> bodySummaryText = new();
            PlayerPin[] playerPins = seriesScores.SelectMany(x => x.PlayerResults)
                                         .GroupBy(x => x.PlayerId)
                                         .Select(x => new PlayerPin(x.Key, (double)x.Sum(y => y.Pins) / x.Count(), x.Sum(y => y.Score), x.Sum(y => y.Pins), x.Count()))
                                         .ToArray();

            // kolla om någon i playerPins är över senaste 20 (måste ha spelat minst 16 serier)
            HashSet<string> aboveLast20Players = new();
            foreach (PlayerPin playerPin in playerPins)
            {
                if (resultsForPlayer.TryGetValue(playerPin.PlayerId, out ResultForPlayerIndex.Result resultForPlayer))
                {
                    bool hasEnoughGames = resultForPlayer.Last5TotalSeries >= 16;
                    double last5Average = (double)resultForPlayer.Last5TotalPins / resultForPlayer.Last5TotalSeries;
                    if (hasEnoughGames
                        && playerPin.SeriesPlayed >= 3
                        && playerPin.Average > last5Average + 2)
                    {
                        _ = aboveLast20Players.Add(playerPin.PlayerId);
                    }
                }
            }

            // summera ihop alla som tagit 4 poäng och utmärk dom också
            HashSet<string> fourPointPlayers = new();
            foreach (PlayerPin playerPin in playerPins)
            {
                if (playerPin.Score == 4)
                {
                    _ = fourPointPlayers.Add(playerPin.PlayerId);
                }
            }

            if (fourPointPlayers.Any())
            {
                PlayerPin[] fourPoints = playerPins.Where(x => fourPointPlayers.Contains(x.PlayerId))
                                           .OrderByDescending(x => x.Pins)
                                           .ToArray();
                string nicknames = nicknameFormatter(fourPoints.Select(x => Tuple.Create(x.PlayerId, 0)).ToArray(), false);
                if (fourPoints.Length < 8)
                {
                    string ending = fourPointPlayers.Count > 1 ? " vardera" : string.Empty;
                    string fourPointsSentence = $"{nicknames} blev matchens viktigaste spelare med 4 vunna serier{ending}.";
                    bodySummaryText.Add(fourPointsSentence);
                }
                else
                {
                    bodySummaryText.Add($"{nicknames} får ses som matchvinnare då alla tog 4 poäng!");
                }
            }

            PlayerPin[] thousand = playerPins.Where(x => x.Pins >= 1000).ToArray();
            if (thousand.Any())
            {
                string nicknames = nicknameFormatter(thousand.Select(x => Tuple.Create(x.PlayerId, 0)).ToArray(), false);
                string sentence = $"Grattis till {nicknames} som uppnådde magiska 1000 poäng!";
                bodySummaryText.Add(sentence);
            }

            if (bodySummaryText.Any())
            {
                bodyText.Add(string.Join(" ", bodySummaryText));
            }

            string lastSentence = string.Join(
                ", ",
                playerPins.OrderByDescending(x => x.Pins).Select(x =>
                {
                    string part;
                    if (x.SeriesPlayed == 4)
                    {
                        part = $"{players[x.PlayerId].Nickname} {x.Pins}";
                    }
                    else
                    {
                        part = $"{players[x.PlayerId].Nickname} {x.Pins} ({x.SeriesPlayed})";
                    }

                    return part;
                }));
            bodyText.Add(lastSentence + ".");
            return bodyText.ToArray();
        }

        private SeriesScores[] GetSeriesScores()
        {
            // calculate series scores
            List<SeriesScores> seriesScores = new();
            int cumulativeScore = 0;
            int cumulativeOpponentScore = 0;
            for (int i = 0; i < matchSeries.Length; i++)
            {
                MatchSerie matchSerie = matchSeries[i];
                ResultSeriesReadModel.Serie opponentSerie = opponentSeries[i];
                SeriesScores seriesScore = new(matchSerie, opponentSerie, cumulativeScore, cumulativeOpponentScore);
                seriesScores.Add(seriesScore);
                cumulativeScore = seriesScore.TeamScoreTotal;
                cumulativeOpponentScore = seriesScore.OpponentScoreTotal;
            }

            return seriesScores.ToArray();
        }
    }
}
