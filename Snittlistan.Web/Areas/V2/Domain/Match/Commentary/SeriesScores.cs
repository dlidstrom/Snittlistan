using System;
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
                matchSerie.Table1.Game1,
                matchSerie.Table1.Game2,
                matchSerie.Table2.Game1,
                matchSerie.Table2.Game2,
                matchSerie.Table3.Game1,
                matchSerie.Table3.Game2,
                matchSerie.Table4.Game1,
                matchSerie.Table4.Game2
            }
            .Select(x => Tuple.Create(x.Player, x.Pins))
            .ToArray();
        }

        public int TeamScoreTotal { get; private set; }
        public int TeamScoreDelta { get; private set; }
        public int OpponentScoreTotal { get; private set; }
        public int OpponentScoreDelta { get; private set; }
        public int SerieNumber { get; private set; }
        public int TeamPins { get; private set; }
        public int OpponentPins { get; private set; }
        public MatchResultType MatchResult { get; private set; }
        public Tuple<string, int>[] PlayerResults { get; private set; }

        public string FormattedResult
        {
            get { return string.Format("{0}-{1}", TeamScoreTotal, OpponentScoreTotal); }
        }

        public string FormattedDeltaResult
        {
            get { return string.Format("{0}-{1}", TeamScoreDelta, OpponentScoreDelta); }
        }
    }
}