using System.Linq;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.Domain.Match.Commentary
{
    public class SeriesScores
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
            MatchResult = TeamPins > OpponentPins
                ? MatchResultType.Win
                : (TeamPins < OpponentPins
                    ? MatchResultType.Loss
                    : MatchResultType.Draw);
            PlayerResults = new[]
            {
                matchSerie.Table1,
                matchSerie.Table2,
                matchSerie.Table3,
                matchSerie.Table4
            }
                .SelectMany(x => new[]
                {
                    new PlayerResult(x.Game1.Player, x.Game1.Pins, x.Score),
                    new PlayerResult(x.Game2.Player, x.Game2.Pins, x.Score)
                })
                .ToArray();
        }

        public int TeamScoreTotal { get; }
        public int TeamScoreDelta { get; }
        public int OpponentScoreTotal { get; }
        public int OpponentScoreDelta { get; }
        public int SerieNumber { get; }
        public int TeamPins { get; }
        public int OpponentPins { get; }
        public MatchResultType MatchResult { get; }
        public PlayerResult[] PlayerResults { get; }

        public string FormattedResult => $"{TeamScoreTotal}-{OpponentScoreTotal}";

        public string FormattedDeltaResult => $"{TeamScoreDelta}-{OpponentScoreDelta}";

        public string FormattedInvertedDeltaResult => $"{OpponentScoreDelta}-{TeamScoreDelta}";
    }
}