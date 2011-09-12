using System.Collections.Generic;
using System.Linq;

namespace Snittlistan.Models
{
	public class Team
	{
		private List<Game> games = new List<Game>();

		public Team(string name, int score)
		{
			Name = name;
			Score = score;
		}

		public string Name { get; set; }

		/// <summary>
		/// Gets the total score.
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