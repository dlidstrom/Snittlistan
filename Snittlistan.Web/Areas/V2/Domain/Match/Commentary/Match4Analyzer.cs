namespace Snittlistan.Web.Areas.V2.Domain.Match.Commentary
{
    using System.Collections.Generic;
    using System.Linq;

    public class Match4Analyzer
    {
        private readonly MatchSerie4[] matchSeries;
        private readonly Dictionary<string, Player> players;

        public Match4Analyzer(MatchSerie4[] matchSeries, Dictionary<string, Player> players)
        {
            this.matchSeries = matchSeries;
            this.players = players;
        }

        public string GetBodyText()
        {
            var seriesScores = GetSeriesScores();
            var playerPins = seriesScores.SelectMany(x => x.PlayerResults)
                                         .GroupBy(x => x.PlayerId)
                                         .Select(x => new PlayerPin(x.Key, (double)x.Sum(y => y.Pins) / x.Count(), x.Sum(y => y.Score), x.Sum(y => y.Pins), x.Count()))
                                         .ToArray();

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

            return lastSentence + ".";
        }

        private Series4Scores[] GetSeriesScores()
        {
            // calculate series scores
            var seriesScores = new List<Series4Scores>();
            foreach (var matchSerie in matchSeries)
            {
                var seriesScore = new Series4Scores(matchSerie);
                seriesScores.Add(seriesScore);
            }

            return seriesScores.ToArray();
        }
    }
}