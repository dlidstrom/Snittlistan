using System;
using System.Collections.Generic;
using System.Linq;
using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.Domain.Match.Commentary
{
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
            var seriesScores = GetSeriesScores();
            var summaryPatterns = SummaryPatterns.Create(players);

            var matches = summaryPatterns.Where(x => x.Matches(seriesScores)).ToArray();
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
            Func<Tuple<string, int>[], bool, string> nicknameFormatter = (ids, includeResult) =>
            {
                Func<string, int, string> formatOne = (nickname, pins) =>
                    includeResult
                    ? $"{nickname} ({pins})"
                    : nickname;
                if (ids.Length == 1)
                {
                    return formatOne(players[ids.First().Item1].Nickname, ids.First().Item2);
                }

                var firstNicknames = string.Join(
                    ", ",
                    ids.Take(ids.Length - 1)
                       .Select(x => formatOne(players[x.Item1].Nickname, x.Item2)));
                var nicknames = $"{firstNicknames} och {formatOne(players[ids.Last().Item1].Nickname, ids.Last().Item2)}";
                return nicknames;
            };

            var bodyText = new List<string>();
            var seriesScores = GetSeriesScores();
            foreach (var seriesScore in seriesScores)
            {
                var seriesText = new List<string>();
                var hasSerieNumber = false;
                var playersContributingToWin = new HashSet<Tuple<string, int>>();
                var diff = seriesScore.TeamPins - seriesScore.OpponentPins;
                if (diff <= 20 && seriesScore.MatchResult == MatchResultType.Win)
                {
                    hasSerieNumber = true;
                    seriesText.Add(
                        $"Serie {seriesScore.SerieNumber} vanns med enbart {diff} pinnar.");
                    foreach (var playerResult in seriesScore.PlayerResults)
                    {
                        if ((seriesScore.TeamPins - playerResult.Pins) / 7 < playerResult.Pins - 20)
                        {
                            playersContributingToWin.Add(Tuple.Create(playerResult.PlayerId, playerResult.Pins));
                        }
                    }

                    if (playersContributingToWin.Any())
                    {
                        var joiner = playersContributingToWin.Count > 1 ? "sina" : "sitt";
                        var nicknames = nicknameFormatter.Invoke(playersContributingToWin.OrderByDescending(x => x.Item2).ToArray(), true);
                        seriesText.Add(
                            $"{nicknames} låg bakom serievinsten med {joiner} fina resultat.");
                    }
                }

                var greatSeries = new HashSet<Tuple<string, int>>();
                foreach (var playerResult in seriesScore.PlayerResults)
                {
                    var lookingFor = Tuple.Create(playerResult.PlayerId, playerResult.Pins);
                    if (playerResult.Pins >= 270 && playersContributingToWin.Contains(lookingFor) == false)
                    {
                        greatSeries.Add(lookingFor);
                    }
                }

                if (greatSeries.Any())
                {
                    var seriesExcept300 = greatSeries.Where(x => x.Item2 != 300).ToArray();
                    var series300 = greatSeries.Where(x => x.Item2 == 300).ToArray();
                    var trail = seriesExcept300.Length > 1 ? "utmärkta insatser" : "utmärkt insats";
                    if (seriesExcept300.Any())
                    {
                        if (!hasSerieNumber)
                        {
                            var sentence = $"I serie {seriesScore.SerieNumber} stod {nicknameFormatter.Invoke(seriesExcept300.OrderByDescending(x => x.Item2).ToArray(), true)} för {trail}.";
                            seriesText.Add(sentence);
                        }
                        else
                        {
                            var sentence = $"{nicknameFormatter.Invoke(seriesExcept300.OrderByDescending(x => x.Item2).ToArray(), true)} stod för bra {trail}.";
                            seriesText.Add(sentence);
                        }
                    }

                    if (series300.Any())
                    {
                        if (!hasSerieNumber)
                        {
                            var sentence = $"I serie {seriesScore.SerieNumber} smällde {nicknameFormatter.Invoke(series300, false)} till med 300!";
                            seriesText.Add(sentence);
                        }
                        else
                        {
                            var sentence = $"{nicknameFormatter.Invoke(series300, false)} smällde till med 300!";
                            seriesText.Add(sentence);
                        }
                    }
                }

                if (seriesText.Any())
                {
                    bodyText.Add(string.Join(" ", seriesText));
                }
            }

            var bodySummaryText = new List<string>();
            var playerPins = seriesScores.SelectMany(x => x.PlayerResults)
                                         .GroupBy(x => x.PlayerId)
                                         .Select(x => new PlayerPin(x.Key, (double)x.Sum(y => y.Pins) / x.Count(), x.Sum(y => y.Score), x.Sum(y => y.Pins), x.Count()))
                                         .ToArray();

            // kolla om någon i playerPins är över senaste 20 (måste ha spelat minst 16 serier)
            var aboveLast20Players = new HashSet<string>();
            foreach (var playerPin in playerPins)
            {
                if (resultsForPlayer.TryGetValue(playerPin.PlayerId, out var resultForPlayer))
                {
                    var hasEnoughGames = resultForPlayer.Last5TotalSeries >= 16;
                    var last5Average = (double)resultForPlayer.Last5TotalPins / resultForPlayer.Last5TotalSeries;
                    if (hasEnoughGames
                        && playerPin.SeriesPlayed >= 3
                        && playerPin.Average > last5Average + 2)
                    {
                        aboveLast20Players.Add(playerPin.PlayerId);
                    }
                }
            }

            // summera ihop alla som tagit 4 poäng och utmärk dom också
            var fourPointPlayers = new HashSet<string>();
            foreach (var playerPin in playerPins)
            {
                if (playerPin.Score == 4)
                {
                    fourPointPlayers.Add(playerPin.PlayerId);
                }
            }

            if (fourPointPlayers.Any())
            {
                var fourPoints = playerPins.Where(x => fourPointPlayers.Contains(x.PlayerId))
                                           .OrderByDescending(x => x.Pins)
                                           .ToArray();
                var nicknames = nicknameFormatter.Invoke(fourPoints.Select(x => Tuple.Create(x.PlayerId, 0)).ToArray(), false);
                if (fourPoints.Length < 8)
                {
                    var ending = fourPointPlayers.Count > 1 ? " vardera" : string.Empty;
                    var fourPointsSentence = $"{nicknames} blev matchens viktigaste spelare med 4 vunna serier{ending}.";
                    bodySummaryText.Add(fourPointsSentence);
                }
                else
                {
                    bodySummaryText.Add($"{nicknames} får ses som matchvinnare då alla tog 4 poäng!");
                }
            }

            var thousand = playerPins.Where(x => x.Pins >= 1000).ToArray();
            if (thousand.Any())
            {
                var nicknames = nicknameFormatter.Invoke(thousand.Select(x => Tuple.Create(x.PlayerId, 0)).ToArray(), false);
                var sentence = $"Grattis till {nicknames} som uppnådde magiska 1000 poäng!";
                bodySummaryText.Add(sentence);
            }

            if (bodySummaryText.Any())
            {
                bodyText.Add(string.Join(" ", bodySummaryText));
            }

            var lastSentence = string.Join(
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
            var seriesScores = new List<SeriesScores>();
            var cumulativeScore = 0;
            var cumulativeOpponentScore = 0;
            for (var i = 0; i < matchSeries.Length; i++)
            {
                var matchSerie = matchSeries[i];
                var opponentSerie = opponentSeries[i];
                var seriesScore = new SeriesScores(matchSerie, opponentSerie, cumulativeScore, cumulativeOpponentScore);
                seriesScores.Add(seriesScore);
                cumulativeScore = seriesScore.TeamScoreTotal;
                cumulativeOpponentScore = seriesScore.OpponentScoreTotal;
            }

            return seriesScores.ToArray();
        }
    }
}