using System;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.Domain
{
    public class Parse4Result
    {
        public Parse4Result(int teamScore, int awayScore, ResultSeries4ReadModel.Serie[] series)
        {
            if (series == null) throw new ArgumentNullException("series");
            Series = series;
            OpponentScore = awayScore;
            TeamScore = teamScore;
        }

        public int TeamScore { get; private set; }

        public int OpponentScore { get; private set; }

        public ResultSeries4ReadModel.Serie[] Series { get; private set; }
    }
}