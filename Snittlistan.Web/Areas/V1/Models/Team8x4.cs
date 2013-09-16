using System.Collections.Generic;
using System.Linq;
using Raven.Imports.Newtonsoft.Json;

namespace Snittlistan.Web.Areas.V1.Models
{
    /// <summary>
    /// Represents a team in a match.
    /// </summary>
    public class Team8x4
    {
        private static readonly Dictionary<int, int[]> HomeScheme = new Dictionary<int, int[]>
        {
            { 0, new[] { 0, 2, 3, 1 } },
            { 1, new[] { 1, 3, 2, 0 } },
            { 2, new[] { 2, 0, 1, 3 } },
            { 3, new[] { 3, 1, 0, 2 } },
        };

        private static readonly Dictionary<int, int[]> AwayScheme = new Dictionary<int, int[]>
        {
            { 0, new[] { 0, 3, 1, 2 } },
            { 1, new[] { 1, 2, 0, 3 } },
            { 2, new[] { 2, 1, 3, 0 } },
            { 3, new[] { 3, 0, 2, 1 } },
        };

        [JsonProperty(PropertyName = "Series")]
        private readonly List<Serie8x4> series;

        /// <summary>
        /// Initializes a new instance of the Team8x4 class.
        /// </summary>
        /// <param name="name">Name of the team.</param>
        /// <param name="score">Total score.</param>
        public Team8x4(string name, int score)
        {
            Name = name;
            Score = score;
            series = new List<Serie8x4>();
        }

        /// <summary>
        /// Initializes a new instance of the Team8x4 class.
        /// </summary>
        /// <param name="name">Name of the team.</param>
        /// <param name="score">Total score.</param>
        /// <param name="series">Series played by team.</param>
        [JsonConstructor]
        public Team8x4(string name, int score, IEnumerable<Serie8x4> series)
        {
            Name = name;
            Score = score;
            this.series = series.ToList();
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the total score.
        /// </summary>
        public int Score { get; private set; }

        /// <summary>
        /// Gets the series.
        /// </summary>
        public IEnumerable<Serie8x4> Series
        {
            get { return series; }
        }

        [JsonProperty]
        private bool HomeTeam { get; set; }

        /// <summary>
        /// H1-B1 H2-B2 H3-B3 H4-B4
        /// H3-B4 H4-B3 H1-B2 H2-B1
        /// H4-B2 H3-B1 H2-B4 H1-B3
        /// H2-B3 H1-B4 H4-B1 H3-B2.
        /// </summary>
        /// <param name="name">Name of team.</param>
        /// <param name="score">Team score.</param>
        /// <param name="series">Team series.</param>
        public static Team8x4 CreateHomeTeam(string name, int score, IEnumerable<Serie8x4> series)
        {
            var enumerable = series as List<Serie8x4> ?? series.ToList();
            var seriesInOrder = new List<Serie8x4>
            {
                CreateSerie(enumerable[0].Tables, 0, 1, 2, 3),
                CreateSerie(enumerable[1].Tables, 2, 3, 0, 1),
                CreateSerie(enumerable[2].Tables, 3, 2, 1, 0),
                CreateSerie(enumerable[3].Tables, 1, 0, 3, 2)
            };

            return new Team8x4(name, score, seriesInOrder) { HomeTeam = true };
        }

        /// <summary>
        /// H1-B1 H2-B2 H3-B3 H4-B4
        /// H3-B4 H4-B3 H1-B2 H2-B1
        /// H4-B2 H3-B1 H2-B4 H1-B3
        /// H2-B3 H1-B4 H4-B1 H3-B2.
        /// </summary>
        /// <param name="name">Name of team.</param>
        /// <param name="score">Team score.</param>
        /// <param name="series">Team series.</param>
        public static Team8x4 CreateAwayTeam(string name, int score, IEnumerable<Serie8x4> series)
        {
            var enumerable = series as List<Serie8x4> ?? series.ToList();
            var seriesInOrder = new List<Serie8x4>
            {
                CreateSerie(enumerable[0].Tables, 0, 1, 2, 3),
                CreateSerie(enumerable[1].Tables, 3, 2, 1, 0),
                CreateSerie(enumerable[2].Tables, 1, 0, 3, 2),
                CreateSerie(enumerable[3].Tables, 2, 3, 0, 1)
            };

            return new Team8x4(name, score, seriesInOrder) { HomeTeam = false };
        }

        /// <summary>
        /// Returns the total pins.
        /// </summary>
        /// <returns>Total pins.</returns>
        public int Pins()
        {
            return Series.Sum(s => s.Pins());
        }

        /// <summary>
        /// Returns the score for a serie.
        /// </summary>
        /// <param name="serie">Serie index (1-based).</param>
        /// <returns></returns>
        public int ScoreFor(int serie)
        {
            return Series.ElementAt(serie - 1).Score();
        }

        /// <summary>
        /// Returns the total pins for a serie.
        /// </summary>
        /// <param name="serie">Serie number (1-based).</param>
        /// <returns>Total pins for the specified serie.</returns>
        public int PinsFor(int serie)
        {
            return Series.ElementAt(serie - 1).Pins();
        }

        /// <summary>
        /// Returns the total pins for a player.
        /// </summary>
        /// <param name="player">Player name.</param>
        /// <returns>Total pins for player in all series.</returns>
        public int PinsForPlayer(string player)
        {
            return Series.Sum(s => s.PinsForPlayer(player));
        }

        /// <summary>
        /// H1-B1 H2-B2 H3-B3 H4-B4
        /// H3-B4 H4-B3 H1-B2 H2-B1
        /// H4-B2 H3-B1 H2-B4 H1-B3
        /// H2-B3 H1-B4 H4-B1 H3-B2.
        /// </summary>
        /// <param name="serie">Serie to fetch.</param>
        /// <param name="pair">Pair to fetch.</param>
        /// <returns>Serie for pair.</returns>
        public Table8x4 TableAt(int serie, int pair)
        {
            if (HomeTeam)
                return series[serie].Tables.ElementAt(HomeScheme[pair][serie]);
            return series[serie].Tables.ElementAt(AwayScheme[pair][serie]);
        }

        private static Serie8x4 CreateSerie(IEnumerable<Table8x4> tables, int i1, int i2, int i3, int i4)
        {
            var enumerable = tables as List<Table8x4> ?? tables.ToList();
            return new Serie8x4(new List<Table8x4>
            {
                enumerable[i1],
                enumerable[i2],
                enumerable[i3],
                enumerable[i4]
            });
        }
    }
}