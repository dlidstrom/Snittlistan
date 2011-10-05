using System.Collections.Generic;
using System.Linq;

namespace Snittlistan.Models
{
	public class Serie
	{
		/// <summary>
		/// Gets or sets the tables.
		/// </summary>
		public List<Table> Tables { get; set; }

		/// <summary>
		/// Returns the total pins for this serie.
		/// </summary>
		/// <returns>Total pins.</returns>
		public int Pins()
		{
			return Tables.Sum(t => t.Pins());
		}

		/// <summary>
		/// Gets the total score of this serie.
		/// </summary>
		/// <returns></returns>
		public int Score()
		{
			return Tables.Sum(t => t.Score);
		}

		/// <summary>
		/// Returns the pins for a player.
		/// </summary>
		/// <param name="player">Player name.</param>
		/// <returns>Pins for player.</returns>
		public int PinsForPlayer(string player)
		{
			return Tables.Sum(t => t.PinsForPlayer(player));
		}
	}
}