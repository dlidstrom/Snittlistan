using System;
using System.Collections.Generic;
using System.Linq;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.Domain.Match.Commentary
{
    public class MatchAnalyzer
    {
        private readonly MatchSerie[] matchSeries;
        private readonly ResultSeriesReadModel.Serie[] opponentSeries;

        public MatchAnalyzer(
            MatchSerie[] matchSeries,
            ResultSeriesReadModel.Serie[] opponentSeries)
        {
            this.matchSeries = matchSeries;
            this.opponentSeries = opponentSeries;
        }

        public string GetSummaryText()
        {
            var seriesScores = GetSeriesScores();
            var summaryPatterns = SummaryPatterns.Create();

            var matches = summaryPatterns.Where(x => x.Matches(seriesScores)).ToArray();
            string matchCommentary;
            if (matches.Length > 1)
            {
                matchCommentary = string.Join(", ", matches.Select(x => string.Format("'{0}'", x.Description)));
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

        public string GetBodyText(Dictionary<string, Player> players)
        {
            Func<Tuple<string, int>[], string> nicknameFormatter = ids =>
            {
                if (ids.Length == 1)
                {
                    return string.Format("{0} ({1})", players[ids.First().Item1].Nickname, ids.First().Item2);
                }

                var firstNicknames = string.Join(
                    ", ",
                    ids.Take(ids.Length - 1)
                       .Select(x => string.Format("{0} ({1})", players[x.Item1].Nickname, x.Item2)));
                var nicknames = string.Format(
                    "{0} och {1} ({2})",
                    firstNicknames,
                    players[ids.Last().Item1].Nickname,
                    ids.Last().Item2);
                return nicknames;
            };

            var bodyText = new List<string>();
            var seriesScores = GetSeriesScores();
            foreach (var seriesScore in seriesScores)
            {
                var hasSerieNumber = false;
                var playersContributingToWin = new HashSet<Tuple<string, int>>();
                var diff = seriesScore.TeamPins - seriesScore.OpponentPins;
                if (diff <= 20 && seriesScore.MatchResult == MatchResultType.Win)
                {
                    hasSerieNumber = true;
                    bodyText.Add(
                        string.Format(
                            "Serie {0} vanns med enbart {1} pinnar.",
                            seriesScore.SerieNumber,
                            diff));
                    foreach (var playerResult in seriesScore.PlayerResults)
                    {
                        if ((seriesScore.TeamPins - playerResult.Item2)/7 < playerResult.Item2 - 20)
                        {
                            playersContributingToWin.Add(playerResult);
                        }
                    }

                    if (playersContributingToWin.Any())
                    {
                        var joiner = playersContributingToWin.Count > 1 ? "sina" : "sitt";
                        bodyText.Add(
                            string.Format(
                                "{0} låg bakom serievinsten med {1} fina resultat.",
                                nicknameFormatter.Invoke(playersContributingToWin.OrderByDescending(x => x.Item2).ToArray()), joiner));
                    }
                }

                var greatSeries = new HashSet<Tuple<string, int>>();
                foreach (var playerResult in seriesScore.PlayerResults)
                {
                    if (playerResult.Item2 >= 270 && playersContributingToWin.Contains(playerResult) == false)
                    {
                        greatSeries.Add(playerResult);
                    }
                }

                if (greatSeries.Any())
                {
                    var trail = greatSeries.Count > 1 ? "insatser" : "insats";
                    if (!hasSerieNumber)
                    {
                        bodyText.Add(
                            string.Format(
                                "I serie {0} stod {1} för bra {2}.",
                                seriesScore.SerieNumber,
                                nicknameFormatter.Invoke(greatSeries.OrderByDescending(x => x.Item2).ToArray()),
                                trail));
                    }
                    else
                    {
                        bodyText.Add(
                            string.Format(
                                "{0} stod för bra {1}.",
                                nicknameFormatter.Invoke(greatSeries.OrderByDescending(x => x.Item2).ToArray()),
                                trail));
                    }
                }
            }

            var result = string.Join(" ", bodyText);
            return result;
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