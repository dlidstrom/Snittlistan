namespace Snittlistan.Models
{
	/// <summary>
	/// Represents a table in a serie.
	/// </summary>
	public class Table
	{
		/// <summary>
		/// Gets or sets the score.
		/// </summary>
		public int Score { get; set; }

		/// <summary>
		/// Gets or sets the first game.
		/// </summary>
		public Game Game1 { get; set; }

		/// <summary>
		/// Gets or sets the second game.
		/// </summary>
		public Game Game2 { get; set; }

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
	}
}
