using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Snittlistan.Models
{
	public class Serie
	{
		/// <summary>
		/// Gets or sets the tables.
		/// </summary>
		public List<Table> Tables { get; set; }

		/// <summary>
		/// Gets the total pins for this serie.
		/// </summary>
		public int Pins
		{
			get
			{
				return Tables.Sum(t => t.Pins);
			}
		}

		/// <summary>
		/// Gets the total score of this serie.
		/// </summary>
		/// <returns></returns>
		public int Score()
		{
			return Tables.Sum(t => t.Score);
		}

		public int PinscoreForPlay(string player)
		{
			return Tables.Sum(t => t.PinscoreForPlayer(player));
		}
	}
}