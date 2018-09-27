using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EventStoreLite;
using Raven.Imports.Newtonsoft.Json;

namespace Snittlistan.Web.Areas.V2.ReadModels
{
    public class ResultSeries4ReadModel : IReadModel
    {
        public ResultSeries4ReadModel()
        {
            Series = new List<Serie>();
        }

        [JsonConstructor]
        private ResultSeries4ReadModel(List<Serie> series)
        {
            Series = series;
        }

        public string Id { get; set; }

        public List<Serie> Series { get; private set; }

        public static string IdFromBitsMatchId(int bitsMatchId, string rosterId)
        {
            if (bitsMatchId != 0)
                return $"ResultSeries4-{bitsMatchId}";
            return $"ResultSeries4-R{rosterId.Substring(8)}";
        }

        public IEnumerable<KeyValuePair<string, List<PlayerGame>>> SortedPlayers()
        {
            var players = Series.SelectMany(x => x.Games)
                .Select(x => x.Player);

            var dictionary = new Dictionary<string, List<PlayerGame>>();
            foreach (var game in players)
            {
                if (dictionary.ContainsKey(game) == false)
                    dictionary.Add(
                        game,
                        new List<PlayerGame>
                        {
                            null,
                            null,
                            null,
                            null
                        });
            }

            for (var i = 0; i < Series.Count; i++)
            {
                var serie = Series[i];
                foreach (var game in serie.Games)
                {
                    dictionary[game.Player][i] = new PlayerGame(game, game.Score);
                }
            }

            var q = from x in dictionary
                    let value = dictionary[x.Key]
                    let series = value.Where(y => y != null)
                    let sum = series.Sum(z => z.Pins)
                    orderby sum descending
                    select x;
            return q;
        }

        public string SerieSum(int i)
        {
            if (Series.Count > i)
            {
                var serieSum = Series[i].Games.Sum(t => t.Pins);
                return serieSum.ToString(CultureInfo.InvariantCulture);
            }

            return string.Empty;
        }

        public string Total()
        {
            if (!Series.Any()) return string.Empty;

            var total = Series.SelectMany(s => s.Games)
                .Sum(t => t.Pins);
            return total.ToString(CultureInfo.InvariantCulture);
        }

        public void Clear()
        {
            Series = new List<Serie>();
        }

        public class Serie
        {
            public Serie()
            {
                Games = new List<Game>
                {
                    new Game(),
                    new Game(),
                    new Game(),
                    new Game()
                };
            }

            [JsonConstructor]
            private Serie(int score, List<Game> games)
            {
                Score = score;
                Games = games;
            }

            public int Score { get; set; }

            public List<Game> Games { get; set; }
        }

        public class Game
        {
            public Game()
            {
                Player = string.Empty;
            }

            public string Player { get; set; }

            public int Pins { get; set; }

            public int Score { get; set; }
        }

        public class PlayerGame
        {
            public PlayerGame(Game game, int score)
            {
                Pins = game.Pins;
                Score = score;
            }

            public int Score { get; }

            public int Pins { get; }
        }
    }
}