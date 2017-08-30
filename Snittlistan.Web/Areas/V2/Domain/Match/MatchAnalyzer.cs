using System;
using System.Collections.Generic;
using System.Linq;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.Domain.Match
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
            var summaryPatterns = CreateSummaryPatterns();

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

        private static SummaryPattern[] CreateSummaryPatterns()
        {
            Func<SeriesScores[], string> seriesFormatter = seriesScores =>
            {
                var result = string.Format(
                    "Serierna slutade {0}.",
                    string.Join(
                        ", ",
                        seriesScores.Select(x => string.Format("{0} ({1}-{2})", x.FormattedDeltaResult, x.TeamPins, x.OpponentPins))));
                return result;
            };

            var summaryPatterns = new[]
            {
                new SummaryPattern("20-0")
                {
                    NumberOfSeries = 4,
                    MatchWon = MatchResult.Won,
                    TeamScore = (teamScore, opponentScore, seriesScores) => teamScore == 20,
                    OpponentScore = (teamScore, opponentScore) => true,
                    Commentary = seriesScores =>
                    {
                        var sentences = new List<string>
                        {
                            string.Format(
                                "Motståndarna hade inget att säga emot då resultatet blev {0}. Stark insats av laget där alltså alla spelare to 4 poäng!",
                                seriesScores[3].FormattedResult)
                        };

                        sentences.Add(seriesFormatter.Invoke(seriesScores));
                        return string.Join(" ", sentences);
                    }
                },
                new SummaryPattern("[14-19]-x")
                {
                    NumberOfSeries = 4,
                    MatchWon = MatchResult.Won,
                    TeamScore = (teamScore, opponentScore, seriesScores) => teamScore >= 14 && teamScore < 20,
                    OpponentScore = (teamScore, opponentScore) => true,
                    Commentary = seriesScores =>
                    {
                        var sentences = new List<string>
                        {
                            string.Format(
                                "Motståndarna blev överkörda med resultatet {0}.",
                                seriesScores[3].FormattedResult),
                            seriesFormatter.Invoke(seriesScores)
                        };
                        return string.Join(" ", sentences);
                    }
                },
                new SummaryPattern("[9-13]-x win")
                {
                    NumberOfSeries = 4,
                    MatchWon = MatchResult.Won,
                    TeamScore = (teamScore, opponentScore, seriesScores)
                        => teamScore < 14 && teamScore > opponentScore,
                    OpponentScore = (teamScore, opponentScore) => true,
                    Commentary = seriesScores =>
                    {
                        var sentences = new List<string>
                        {
                            string.Format(
                                "Matchen vanns med resultatet {0}.",
                                seriesScores[3].FormattedResult)
                        };
                        var greatSeries = seriesScores.FirstOrDefault(x => x.TeamScoreDelta == 5 && x.TeamScoreTotal <= 10);
                        if (greatSeries != null)
                        {
                            sentences.Add(
                                string.Format(
                                    "Grunden till vinsten lades i serie {0} då poängställningen var {1} efter {2}.",
                                    greatSeries.SerieNumber,
                                    greatSeries.FormattedResult,
                                    greatSeries.FormattedDeltaResult));
                        }

                        sentences.Add(seriesFormatter.Invoke(seriesScores));
                        return string.Join(" ", sentences);
                    }
                },
                new SummaryPattern("all 4 series losses")
                {
                    NumberOfSeries = 4,
                    MatchWon = MatchResult.Loss,
                    TeamScore = (teamScore, opponentScore, seriesScores) => true,
                    OpponentScore = (teamScore, opponentScore) => true,
                    Commentary = seriesScores => "4 series loss, all results"
                },
                new SummaryPattern("all 4 series draws")
                {
                    NumberOfSeries = 4,
                    MatchWon = MatchResult.Draw,
                    TeamScore = (teamScore, opponentScore, seriesScores) => true,
                    OpponentScore = (teamScore, opponentScore) => true,
                    Commentary = seriesScores =>
                    {
                        var sentences = new List<string>();
                        var lastSeries = seriesScores.Last();
                        var lastLastSeries = seriesScores[seriesScores.Length-2];
                        var lastLastLastSeries = seriesScores[seriesScores.Length-3];
                        if (lastLastLastSeries.OpponentScoreTotal >= 7)
                        {
                            sentences.Add(
                                string.Format(
                                    "Laget stod för en fin upphämtning då det stod {0} efter halva matchen.",
                                    lastLastLastSeries.FormattedResult));
                        }

                        if (lastLastSeries.OpponentScoreTotal < lastLastSeries.TeamScoreTotal)
                        {
                            if (lastLastSeries.OpponentScoreDelta >= 4)
                            {
                                sentences.Add(
                                    string.Format(
                                        "Det stod {0} efter 3 serier men motståndarna ryckte i sista serien och laget fick nöja sig med oavgjort.",
                                        lastLastSeries.FormattedResult));
                            }
                            else
                            {
                                sentences.Add("Matchen slutade oavgjort efter att motståndarna vann sista serien.");
                            }
                        }
                        else if (lastLastSeries.OpponentScoreTotal > lastLastSeries.TeamScoreTotal)
                        {
                            if (lastLastSeries.OpponentScoreTotal == 10)
                            {
                                sentences.Add("Det stod 5-10 efter 3 serier. Men i sista serien kördes motståndarna över för ett oavgjort resultat!");
                            }
                            else
                            {
                                sentences.Add("Matchen slutade oavgjort efter vinst i sista serien.");
                            }
                        }
                        else
                        {
                            sentences.Add(
                                string.Format(
                                    "Matchen slutade oavgjort med ovanliga resultatet {0}.",
                                    lastSeries.FormattedResult));
                        }

                        sentences.Add(seriesFormatter.Invoke(seriesScores));
                        return string.Join(" ", sentences);
                    }
                },
                new SummaryPattern("all 3 series wins")
                {
                    NumberOfSeries = 3,
                    MatchWon = MatchResult.Won,
                    TeamScore = (teamScore, opponentScore, seriesScores) => true,
                    OpponentScore = (teamScore, opponentScore) => true,
                    Commentary = seriesScores => string.Format(
                        "Matchen vanns redan efter 3 serier med resultatet {0}.",
                        seriesScores[2].FormattedResult)
                },
                new SummaryPattern("all 1 series wins")
                {
                    NumberOfSeries = 1,
                    MatchWon = MatchResult.Won,
                    TeamScore = (teamScore, opponentScore, seriesScores) => true,
                    OpponentScore = (teamScore, opponentScore) => true,
                    Commentary = seriesScores => ""
                }
            };

            return summaryPatterns;
        }

        private class SeriesScores
        {
            public SeriesScores(
                MatchSerie matchSerie,
                ResultSeriesReadModel.Serie opponentSerie,
                int cumulativeScore,
                int cumulativeOpponentScore)
            {
                var teamScore = 0;
                var opponentScore = 0;
                if (matchSerie.TeamTotal > opponentSerie.TeamTotal)
                    teamScore = 1;
                else if (matchSerie.TeamTotal < opponentSerie.TeamTotal)
                    opponentScore = 1;

                TeamScoreDelta = matchSerie.Table1.Score
                                 + matchSerie.Table2.Score
                                 + matchSerie.Table3.Score
                                 + matchSerie.Table4.Score
                                 + teamScore;
                TeamScoreTotal = TeamScoreDelta + cumulativeScore;
                OpponentScoreDelta = opponentSerie.Tables.Sum(x => x.Score) + opponentScore;
                OpponentScoreTotal = OpponentScoreDelta + cumulativeOpponentScore;
                SerieNumber = matchSerie.SerieNumber;
                TeamPins = matchSerie.TeamTotal;
                OpponentPins = opponentSerie.TeamTotal;
            }

            public int TeamScoreTotal { get; private set; }
            public int TeamScoreDelta { get; private set; }
            public int OpponentScoreTotal { get; private set; }
            public int OpponentScoreDelta { get; private set; }
            public int SerieNumber { get; private set; }
            public int TeamPins { get; private set; }
            public int OpponentPins { get; private set; }

            public string FormattedResult
            {
                get { return string.Format("{0}-{1}", TeamScoreTotal, OpponentScoreTotal); }
            }

            public string FormattedDeltaResult
            {
                get { return string.Format("{0}-{1}", TeamScoreDelta, OpponentScoreDelta); }
            }
        }

        private class SummaryPattern
        {
            public SummaryPattern(string description)
            {
                Description = description;
            }

            public int NumberOfSeries { get; set; }

            public MatchResult MatchWon { get; set; }

            public Func<int, int, SeriesScores[], bool> TeamScore { get; set; }

            public Func<int, int, bool> OpponentScore { get; set; }

            public Func<SeriesScores[], string> Commentary { get; set; }

            public string Description { get; private set; }

            public bool Matches(
                SeriesScores[] seriesScores)
            {
                var teamScore = seriesScores.Last().TeamScoreTotal;
                var opponentScore = seriesScores.Last().OpponentScoreTotal;
                var matchWon = teamScore > opponentScore
                    ? MatchResult.Won
                    : (teamScore < opponentScore ? MatchResult.Loss : MatchResult.Draw);
                var numberOfSeries = seriesScores.Length;

                var matches = numberOfSeries == NumberOfSeries
                              && matchWon == MatchWon
                              && TeamScore.Invoke(teamScore, opponentScore, seriesScores)
                              && OpponentScore.Invoke(teamScore, opponentScore);

                return matches;
            }
        }

        private enum MatchResult
        {
            Won,
            Loss,
            Draw
        }
    }
}