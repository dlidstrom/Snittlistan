namespace Snittlistan.Web.Areas.V1.Models
{
    /// <summary>
    /// Represents a game in a serie.
    /// </summary>
    public class Game4x4
    {
        /// <summary>
        /// Initializes a new instance of the Game4x4 class.
        /// </summary>
        /// <param name="player">Player name.</param>
        /// <param name="pins">Pins of the game.</param>
        /// <param name="score">Score of the game.</param>
        public Game4x4(string player, int pins, int score)
        {
            Player = player;
            Pins = pins;
            Score = score;
        }

        /// <summary>
        /// Gets the player name.
        /// </summary>
        public string Player { get; private set; }

        /// <summary>
        /// Gets the pins.
        /// </summary>
        public int Pins { get; private set; }

        /// <summary>
        /// Gets the score.
        /// </summary>
        public int Score { get; private set; }

        /// <summary>
        /// Gets or sets the number of strikes.
        /// </summary>
        public int? Strikes { get; set; }

        /// <summary>
        /// Gets or sets the number of misses.
        /// </summary>
        public int? Misses { get; set; }

        /// <summary>
        /// Gets or sets the number of one-pin misses.
        /// </summary>
        public int? OnePinMisses { get; set; }

        /// <summary>
        /// Gets or sets the number of splits.
        /// </summary>
        public int? Splits { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether all frames were covered (no misses).
        /// </summary>
        public bool CoveredAll { get; set; }
    }
}