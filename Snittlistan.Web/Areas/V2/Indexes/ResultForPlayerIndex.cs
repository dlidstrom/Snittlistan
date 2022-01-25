using Raven.Client.Indexes;
using Snittlistan.Web.Areas.V2.ReadModels;

#nullable enable

namespace Snittlistan.Web.Areas.V2.Indexes;
public class ResultForPlayerIndex : AbstractIndexCreationTask<ResultForPlayerReadModel, ResultForPlayerIndex.Result>
{
    public ResultForPlayerIndex()
    {
        Map = results => from result in results
                         select new
                         {
                             result.PlayerId,
                             result.Season,
                             result.TotalSeries,
                             result.TotalPins,
                             result.TotalScore,
                             Last5 = Enumerable.Repeat(new
                             {
                                 result.Date,
                                 result.TotalSeries,
                                 result.TotalPins
                             }, 1),
                             Last5TotalSeries = result.TotalSeries,
                             Last5TotalPins = result.TotalPins
                         };

        Reduce = results => from result in results
                            group result by new
                            {
                                result.PlayerId,
                                result.Season
                            } into g
                            select new Result
                            {
                                PlayerId = g.Key.PlayerId,
                                Season = g.Key.Season,
                                TotalSeries = g.Sum(x => x.TotalSeries),
                                TotalPins = g.Sum(x => x.TotalPins),
                                TotalScore = g.Sum(x => x.TotalScore),
                                Last5 = g.SelectMany(x => x.Last5)
                                    .OrderByDescending(x => x.Date)
                                    .Take(5),
                                Last5TotalPins = g.SelectMany(x => x.Last5)
                                    .OrderByDescending(x => x.Date)
                                    .Take(5)
                                    .Sum(x => x.TotalPins),
                                Last5TotalSeries = g.SelectMany(x => x.Last5)
                                    .OrderByDescending(x => x.Date)
                                    .Take(5)
                                    .Sum(x => x.TotalSeries)
                            };
    }

    public class Game
    {
        public DateTimeOffset Date { get; set; }

        public int TotalSeries { get; set; }

        public int TotalPins { get; set; }
    }

    public class Result
    {
        public string PlayerId { get; set; } = null!;

        public int Season { get; set; }

        public int TotalSeries { get; set; }

        public int TotalPins { get; set; }

        public int TotalScore { get; set; }

        public IEnumerable<Game> Last5 { get; set; } = null!;

        public int Last5TotalSeries { get; set; }

        public int Last5TotalPins { get; set; }
    }
}
