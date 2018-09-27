namespace Snittlistan.Web.Areas.V2.Domain.Match.Commentary
{
    using System.Linq;

    public class Series4Scores
    {
        public Series4Scores(MatchSerie4 matchSerie)
        {
            PlayerResults = new[]
                            {
                                matchSerie.Game1,
                                matchSerie.Game2,
                                matchSerie.Game3,
                                matchSerie.Game4
                            }
                            .Select(x => new PlayerResult(x.Player, x.Pins, x.Score))
                            .ToArray();
        }

        public PlayerResult[] PlayerResults { get; }
    }
}