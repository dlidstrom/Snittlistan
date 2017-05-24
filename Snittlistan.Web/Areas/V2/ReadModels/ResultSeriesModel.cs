using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EventStoreLite;

namespace Snittlistan.Web.Areas.V2.ReadModels
{
    public class ResultSeriesReadModel : IReadModel
    {
        public ResultSeriesReadModel()
        {
            Series = new List<Serie>();
        }

        public string Id { get; set; }

        public List<Serie> Series { get; private set; }

        public static string IdFromBitsMatchId(int id)
        {
            return "Series-" + id;
        }

        public KeyValuePair<string, List<PlayerGame>>[] SortedPlayers()
        {
            var first = Series.SelectMany(x => x.Tables)
                .Select(x => x.Game1);
            var second = Series.SelectMany(x => x.Tables)
                .Select(x => x.Game2);

            var combined = first.Concat(second).ToList();
            var dictionary = new Dictionary<string, List<PlayerGame>>();
            foreach (var game in combined)
            {
                if (dictionary.ContainsKey(game.Player) == false)
                    dictionary.Add(
                        game.Player,
                        new List<PlayerGame>
                        {
                            null,
                            null,
                            null,
                            null
                        });
            }

            for (int i = 0; i < Series.Count; i++)
            {
                var serie = Series[i];
                foreach (var table in serie.Tables)
                {
                    var game1 = table.Game1;
                    var game2 = table.Game2;
                    dictionary[game1.Player][i] = new PlayerGame(game1, table.Score);
                    dictionary[game2.Player][i] = new PlayerGame(game2, table.Score);
                }
            }

            var q = from x in dictionary
                    let value = dictionary[x.Key]
                    let series = value.Where(y => y != null)
                    let sum = series.Sum(z => z.Pins)
                    orderby sum descending
                    select x;
            var result = q.ToArray();
            return result;
        }

        public string SerieSum(int i)
        {
            if (Series.Count > i)
            {
                var serieSum = Series[i].Tables.Sum(t => t.Game1.Pins + t.Game2.Pins);
                return serieSum.ToString(CultureInfo.InvariantCulture);
            }

            return string.Empty;
        }

        public string Total()
        {
            if (!Series.Any()) return string.Empty;

            var total = Series.SelectMany(s => s.Tables)
                .Sum(t => t.Game1.Pins + t.Game2.Pins);
            return total.ToString(CultureInfo.InvariantCulture);
        }

        public class Serie
        {
            public Serie()
            {
                Tables = new List<Table> {
                                             new Table(),
                                             new Table(),
                                             new Table(),
                                             new Table()
                                         };
            }

            public List<Table> Tables { get; set; }
        }

        public class Table
        {
            public Table()
            {
                Game1 = new Game();
                Game2 = new Game();
            }

            public int Score { get; set; }

            public Game Game1 { get; set; }

            public Game Game2 { get; set; }
        }

        public class Game
        {
            public Game()
            {
                Player = string.Empty;
            }

            public string Player { get; set; }

            public int Pins { get; set; }

            public int Strikes { get; set; }

            public int Spares { get; set; }
        }

        public class PlayerGame
        {
            public PlayerGame(Game game, int score)
            {
                Pins = game.Pins;
                Score = score;
                Strikes = game.Strikes;
                Spares = game.Spares;
            }

            public int Score { get; private set; }

            public int Pins { get; private set; }

            public int Strikes { get; private set; }

            public int Spares { get; private set; }
        }
    }
}