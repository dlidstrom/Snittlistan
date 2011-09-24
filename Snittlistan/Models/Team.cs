using System.Collections.Generic;
using System.Linq;

namespace Snittlistan.Models
{
	public class Team
	{
		private List<Game> games = new List<Game>();

		/// <summary>
		/// Initializes a new instance of the Team class.
		/// </summary>
		/// <param name="name">Name of the team.</param>
		/// <param name="score">Total score.</param>
		public Team(string name, int score)
		{
			Name = name;
			Score = score;
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the total score.
		/// </summary>
		public int Score { get; set; }

		/// <summary>
		/// Gets the total pins.
		/// </summary>
		public int Pins
		{
			get
			{
				return games.Sum(g => g.Pins);
			}
		}

		public IEnumerable<Game> Games
		{
			get
			{
				return games;
			}
		}

		public int ScoreFor(int serie)
		{
			return games.Where(g => g.Serie == serie).Sum(g => g.Score) / 2;
		}

		public int PinsFor(int serie)
		{
			return games.Where(g => g.Serie == serie).Sum(g => g.Pins);
		}

		public void AddGame(Game game)
		{
			games.Add(game);
		}

		public int PinscoreForPlayer(string player)
		{
			return games.Where(g => g.Player == player).Sum(g => g.Pins);
		}
	}
}