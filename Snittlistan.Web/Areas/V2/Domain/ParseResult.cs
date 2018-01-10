using System;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.Domain
{
    public class ParseResult
    {
        public ParseResult(
            int teamScore,
            int opponentScore,
            ResultSeriesReadModel.Serie[] series,
            ResultSeriesReadModel.Serie[] opponentSeries)
        {
            TeamScore = teamScore;
            OpponentScore = opponentScore;
            Series = series ?? throw new ArgumentNullException(nameof(series));
            OpponentSeries = opponentSeries ?? throw new ArgumentNullException(nameof(opponentSeries));
        }

        public int TeamScore { get; private set; }

        public int OpponentScore { get; private set; }

        public ResultSeriesReadModel.Serie[] Series { get; private set; }

        public ResultSeriesReadModel.Serie[] OpponentSeries { get; private set; }
    }
}