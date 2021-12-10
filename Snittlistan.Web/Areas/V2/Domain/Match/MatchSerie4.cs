using Raven.Imports.Newtonsoft.Json;

namespace Snittlistan.Web.Areas.V2.Domain.Match;
public class MatchSerie4
{
    public MatchSerie4(int serieNumber, IReadOnlyList<MatchGame4> games)
    {
        if (games == null)
        {
            throw new ArgumentNullException(nameof(games));
        }

        if (games.Count != 4)
        {
            throw new ArgumentException("games");
        }

        SerieNumber = serieNumber;

        var players = new HashSet<string>(games.AsEnumerable().Select(x => x.Player));

        if (players.Count != 4)
        {
            throw new MatchException("Serie must have 4 different players");
        }

        Game1 = games[0];
        Game2 = games[1];
        Game3 = games[2];
        Game4 = games[3];
    }

    [JsonConstructor]
    private MatchSerie4(int serieNumber, MatchGame4 game1, MatchGame4 game2, MatchGame4 game3, MatchGame4 game4)
    {
        SerieNumber = serieNumber;
        Game1 = game1;
        Game2 = game2;
        Game3 = game3;
        Game4 = game4;
    }

    public int SerieNumber { get; }

    public MatchGame4 Game1 { get; }

    public MatchGame4 Game2 { get; }

    public MatchGame4 Game3 { get; }

    public MatchGame4 Game4 { get; }
}
