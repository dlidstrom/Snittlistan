namespace Snittlistan.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a table in a serie.
    /// </summary>
    public class Table8x4
    {
        [JsonProperty(PropertyName = "Games")]
        private readonly List<Game8x4> games;

        /// <summary>
        /// Initializes a new instance of the Table8x4 class.
        /// </summary>
        public Table8x4()
        {
            games = new List<Game8x4>
            {
                new Game8x4(string.Empty, 0),
                new Game8x4(string.Empty, 0)
            };
        }

        /// <summary>
        /// Initializes a new instance of the Table8x4 class.
        /// </summary>
        /// <param name="games">Games in the table.</param>
        [JsonConstructor]
        public Table8x4(IEnumerable<Game8x4> games)
        {
            this.games = games.ToList();
        }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Gets or sets the first game.
        /// </summary>
        [JsonIgnore]
        public Game8x4 Game1
        {
            get { return games[0]; }
            set { games[0] = value; }
        }

        /// <summary>
        /// Gets or sets the second game.
        /// </summary>
        [JsonIgnore]
        public Game8x4 Game2
        {
            get { return games[1]; }
            set { games[1] = value; }
        }

        /// <summary>
        /// Gets the games list.
        /// </summary>
        public IEnumerable<Game8x4> Games
        {
            get { return games; }
        }

        /// <summary>
        /// Returns the total pins by both players.
        /// </summary>
        /// <returns>Total pins.</returns>
        public int Pins()
        {
            return Game1.Pins + Game2.Pins;
        }

        /// <summary>
        /// Gets the pins for one of the players.
        /// </summary>
        /// <param name="player">Player name.</param>
        /// <returns>Pins for player.</returns>
        public int PinsForPlayer(string player)
        {
            if (Game1.Player == player)
                return Game1.Pins;
            else if (Game2.Player == player)
                return Game2.Pins;
            else
                return 0;
        }

        public override string ToString()
        {
            return string.Format("{0};{1}", Game1, Game2);
        }
    }
}
