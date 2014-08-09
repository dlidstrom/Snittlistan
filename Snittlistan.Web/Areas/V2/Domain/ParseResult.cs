using System;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.Domain
{
    public class ParseResult
    {
        public ParseResult(int teamScore, int awayScore, ResultSeriesReadModel.Serie[] series)
        {
            if (series == null) throw new ArgumentNullException("series");
            Series = series;
            OpponentScore = awayScore;
            TeamScore = teamScore;
        }

        public int TeamScore { get; private set; }

        public int OpponentScore { get; private set; }

        public ResultSeriesReadModel.Serie[] Series { get; private set; }
    }
}