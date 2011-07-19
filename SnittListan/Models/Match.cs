using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SnittListan.Models
{
	public class Match
	{
		private List<Serie> series;

		public Match(int id, string place, DateTime date, List<Serie> series)
		{
			Id = id;
			Place = place;
			Date = date;
			this.series = series;
		}

		public int Id { get; private set; }
		public string Place { get; private set; }
		public DateTime Date { get; private set; }

		public int NumberOfSeries
		{
			get
			{
				return series.Count;
			}
		}

		public int PinScoreForTeam()
		{
			return series.Sum(s => s.Games.Sum(g => g.PinScore));
		}

		public int PinscoreForTeam(int serie)
		{
			return series[serie - 1].Games.Sum(g => g.PinScore);
		}

		public int PinscoreForPlayer(string player)
		{
			return series.Sum(s => s.Games.Where(g => g.Player == player).Sum(g => g.PinScore));
		}

		public int LaneScoreForTeam()
		{
			return series.Sum(s => s.Games.Sum(g => g.LaneScore)) / 2;
		}

		public int LaneScoreForTeam(int serie)
		{
			return series[serie - 1].Games.Sum(g => g.LaneScore) / 2;
		}
	}
}