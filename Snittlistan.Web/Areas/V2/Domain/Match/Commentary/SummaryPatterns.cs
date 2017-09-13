using System;
using System.Collections.Generic;
using System.Linq;

namespace Snittlistan.Web.Areas.V2.Domain.Match.Commentary
{
    public static class SummaryPatterns
    {
        public static SummaryPattern[] Create(Dictionary<string, Player> players)
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
                    MatchWon = MatchResultType.Win,
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
                    MatchWon = MatchResultType.Win,
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
                    MatchWon = MatchResultType.Win,
                    TeamScore = (teamScore, opponentScore, seriesScores)
                        => teamScore < 14 && teamScore > opponentScore,
                    OpponentScore = (teamScore, opponentScore) => true,
                    Commentary = seriesScores =>
                    {
                        var lastLastLastSeries = seriesScores[seriesScores.Length-3];
                        var lastLastSeries = seriesScores[seriesScores.Length-2];
                        var lastSeries = seriesScores.Last();
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
                        else if (lastLastLastSeries.OpponentScoreTotal > 5)
                        {
                            sentences.Add(
                                string.Format(
                                    "Laget stod för en stark upphämtning då matchen vändes i serie 2 när det stod {0}.",
                                    lastLastLastSeries.FormattedResult));
                        }

                        if (lastSeries.OpponentScoreDelta > lastSeries.TeamScoreDelta)
                        {
                            var winningPlayers = lastSeries.PlayerResults.Where(x => x.Score > 0);
                            var nicknames = string.Join(
                                " och ",
                                winningPlayers.Select(x => players[x.PlayerId].Nickname));
                            var sentence = string.Format(
                                "{0} avgjorde matchen med en stark insats i sista serien.",
                                nicknames);
                            sentences.Add(sentence);
                        }

                        if (lastLastSeries.OpponentScoreTotal == 7
                            && lastLastSeries.TeamScoreTotal == 8)
                        {
                            sentences.Add("Laget avgjorde matchen med en stark avslutning i sista serien.");
                        }

                        sentences.Add(seriesFormatter.Invoke(seriesScores));
                        return string.Join(" ", sentences);
                    }
                },
                new SummaryPattern("all 4 series losses")
                {
                    NumberOfSeries = 4,
                    MatchWon = MatchResultType.Loss,
                    TeamScore = (teamScore, opponentScore, seriesScores) => true,
                    OpponentScore = (teamScore, opponentScore) => true,
                    Commentary = seriesScores =>
                    {
                        var sentences = new List<string>
                        {
                            string.Format(
                                "Matchen slutade {0}.",
                                seriesScores[3].FormattedResult)
                        };

                        var lossSeries = seriesScores.Skip(1).FirstOrDefault(x => x.TeamScoreDelta == 0
                            && x.TeamScoreTotal <= 5);
                        if (lossSeries != null)
                        {
                            var sentence = string.Format(
                                "Motståndarna lade grunden till vinsten genom att göra {0} i serie {1} vilket resulterade i poängställningen {2}.",
                                lossSeries.FormattedInvertedDeltaResult,
                                lossSeries.SerieNumber,
                                lossSeries.FormattedResult);
                            sentences.Add(sentence);
                        }

                        sentences.Add(seriesFormatter.Invoke(seriesScores));
                        return string.Join(" ", sentences);
                    }
                },
                new SummaryPattern("all 4 series draws")
                {
                    NumberOfSeries = 4,
                    MatchWon = MatchResultType.Draw,
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
                    MatchWon = MatchResultType.Win,
                    TeamScore = (teamScore, opponentScore, seriesScores) => true,
                    OpponentScore = (teamScore, opponentScore) => true,
                    Commentary = seriesScores => string.Format(
                        "Matchen vanns redan efter 3 serier med resultatet {0}.",
                        seriesScores[2].FormattedResult)
                },
                new SummaryPattern("all 1 series wins")
                {
                    NumberOfSeries = 1,
                    MatchWon = MatchResultType.Win,
                    TeamScore = (teamScore, opponentScore, seriesScores) => true,
                    OpponentScore = (teamScore, opponentScore) => true,
                    Commentary = seriesScores => ""
                }
            };

            return summaryPatterns;
        }
    }
}