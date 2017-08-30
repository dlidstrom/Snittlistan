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
            if (series == null) throw new ArgumentNullException("series");
            if (opponentSeries == null) throw new ArgumentNullException("opponentSeries");
            TeamScore = teamScore;
            OpponentScore = opponentScore;
            Series = series;
            OpponentSeries = opponentSeries;
        }

        public int TeamScore { get; private set; }

        public int OpponentScore { get; private set; }

        public ResultSeriesReadModel.Serie[] Series { get; private set; }

        public ResultSeriesReadModel.Serie[] OpponentSeries { get; private set; }
    }
}