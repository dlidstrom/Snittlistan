namespace Snittlistan.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Raven.Imports.Newtonsoft.Json;

    /// <summary>
    /// Represents a team in a match.
    /// </summary>
    public class Team4x4
    {
        [JsonProperty(PropertyName = "Series")]
        private readonly List<Serie4x4> series;

        /// <summary>
        /// Initializes a new instance of the Team4x4 class.
        /// </summary>
        /// <param name="name">Name of the team.</param>
        /// <param name="score">Total score.</param>
        public Team4x4(string name, int score)
        {
            Name = name;
            Score = score;
            series = new List<Serie4x4>();
        }

        /// <summary>
        /// Initializes a new instance of the Team4x4 class.
        /// </summary>
        /// <param name="name">Name of the team.</param>
        /// <param name="score">Total score.</param>
        /// <param name="series">Series played by team.</param>
        [JsonConstructor]
        public Team4x4(string name, int score, IEnumerable<Serie4x4> series)
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
        public IEnumerable<Serie4x4> Series
        {
            get { return series; }
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
    }
}