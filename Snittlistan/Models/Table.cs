namespace Snittlistan.Models
{
	using System.Collections.Generic;
	using Newtonsoft.Json;

	/// <summary>
	/// Represents a table in a serie.
	/// </summary>
	public class Table
	{
		[JsonProperty(PropertyName = "Games")]
		private List<Game> games;

		/// <summary>
		/// Initializes a new instance of the Table class.
		/// </summary>
		public Table()
		{
			games = new List<Game>
			{
				new Game(string.Empty, 0),
				new Game(string.Empty, 0)
			};
		}

		/// <summary>
		/// Initializes a new instance of the Table class.
		/// </summary>
		/// <param name="games">Games in the table.</param>
		[JsonConstructor]
		public Table(IEnumerable<Game> games)
		{
			this.games = new List<Game>(games);
		}

		/// <summary>
		/// Gets or sets the score.
		/// </summary>
		public int Score { get; set; }

		/// <summary>
		/// Gets or sets the first game.
		/// </summary>
		[JsonIgnore]
		public Game Game1
		{
			get { return games[0]; }
			set { games[0] = value; }
		}

		/// <summary>
		/// Gets or sets the second game.
		/// </summary>
		[JsonIgnore]
		public Game Game2
		{
			get { return games[1]; }
			set { games[1] = value; }
		}

		/// <summary>
		/// Gets the games list.
		/// </summary>
		public IEnumerable<Game> Games
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
	}
}
