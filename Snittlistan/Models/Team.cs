using System.Collections.Generic;
using System.Linq;

namespace Snittlistan.Models
{
	public class Team
	{
		/// <summary>
		/// Initializes a new instance of the Team class.
		/// </summary>
		/// <param name="name">Name of the team.</param>
		/// <param name="score">Total score.</param>
		public Team(string name, int score)
		{
			Name = name;
			Score = score;
			Series = new List<Serie>();
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
		/// Gets or sets the series.
		/// </summary>
		public List<Serie> Series { get; set; }

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
			return Series[serie - 1].Score();
		}

		/// <summary>
		/// Returns the total pins for a serie.
		/// </summary>
		/// <param name="serie">Serie number (1-based).</param>
		/// <returns>Total pins for the specified serie.</returns>
		public int PinsFor(int serie)
		{
			return Series[serie - 1].Pins();
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