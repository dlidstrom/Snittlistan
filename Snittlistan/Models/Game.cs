namespace Snittlistan.Models
{
	/// <summary>
	/// Represents a game in a table.
	/// </summary>
	public class Game
	{
		/// <summary>
		/// Initializes a new instance of the Game class.
		/// </summary>
		/// <param name="player">Player name.</param>
		/// <param name="pins">Pins of the game.</param>
		public Game(string player, int pins)
		{
			Player = player;
			Pins = pins;
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
		/// Gets the strikes.
		/// </summary>
		public int Strikes { get; private set; }

		/// <summary>
		/// Gets the misses.
		/// </summary>
		public int Misses { get; private set; }

		/// <summary>
		/// Gets the one-pin misses.
		/// </summary>
		public int OnePinMisses { get; private set; }

		/// <summary>
		/// Gets the splits.
		/// </summary>
		public int Splits { get; private set; }

		/// <summary>
		/// Gets a value indicating whether all frames were covered (no misses).
		/// </summary>
		public bool CoveredAll { get; private set; }
	}
}