namespace Snittlistan.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Raven.Imports.Newtonsoft.Json;

    /// <summary>
    /// Represents a serie in a 4-vs-4 match.
    /// </summary>
    public class Serie4x4
    {
        [JsonProperty(PropertyName = "Games")]
        private readonly List<Game4x4> games;

        /// <summary>
        /// Initializes a new instance of the Serie4x4 class.
        /// </summary>
        /// <param name="games">Games of the serie.</param>
        public Serie4x4(IEnumerable<Game4x4> games)
        {
            this.games = games.ToList();
        }

        /// <summary>
        /// Gets the games.
        /// </summary>
        public IEnumerable<Game4x4> Games
        {
            get { return games; }
        }

        /// <summary>
        /// Returns the total pins for this serie.
        /// </summary>
        /// <returns>Total pins.</returns>
        public int Pins()
        {
            return Games.Sum(g => g.Pins);
        }

        /// <summary>
        /// Gets the total score of this serie.
        /// </summary>
        /// <returns></returns>
        public int Score()
        {
            return Games.Sum(g => g.Score);
        }

        /// <summary>
        /// Returns the pins for a player.
        /// </summary>
        /// <param name="player">Player name.</param>
        /// <returns>Pins for player.</returns>
        public int PinsForPlayer(string player)
        {
            return Games.Where(g => g.Player == player).Sum(g => g.Pins);
        }
    }
}