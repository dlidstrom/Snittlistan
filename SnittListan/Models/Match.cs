using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace SnittListan.Models
{
	public class Match
	{
		public Match(
			int id,
			string place,
			DateTime date,
			bool homeGame,
			string homeTeam,
			string oppTeam,
			int oppTeamLaneScore,
			List<Serie> series)
		{
			Id = id;
			Place = place;
			Date = date;
			HomeGame = homeGame;
			HomeTeam = homeTeam;
			OppTeam = oppTeam;
			OppTeamLaneScore = oppTeamLaneScore;
			Series = series;
		}

		public int Id { get; private set; }
		public string Place { get; private set; }
		public DateTime Date { get; private set; }
		public bool HomeGame { get; private set; }
		public string HomeTeam { get; private set; }
		public string OppTeam { get; private set; }
		public int OppTeamLaneScore { get; private set; }
		public List<Serie> Series { get; private set; }

		[JsonIgnore]
		public int NumberOfSeries
		{
			get
			{
				return Series.Count;
			}
		}

		public int PinScoreForTeam()
		{
			return Series.Sum(s => s.Games.Sum(g => g.PinScore));
		}

		public int PinscoreForTeam(int serie)
		{
			return Series[serie - 1].Games.Sum(g => g.PinScore);
		}

		public int PinscoreForPlayer(string player)
		{
			return Series.Sum(s => s.Games.Where(g => g.Player == player).Sum(g => g.PinScore));
		}

		public int LaneScoreForTeam()
		{
			return Series.Sum(s => s.Games.Sum(g => g.LaneScore)) / 2;
		}

		public int LaneScoreForTeam(int serie)
		{
			return Series[serie - 1].Games.Sum(g => g.LaneScore) / 2;
		}

		public string FormattedLaneScore()
		{
			if (HomeGame)
				return string.Format("{0}-{1}", LaneScoreForTeam(), OppTeamLaneScore);
			else
				return string.Format("{0}-{1}", OppTeamLaneScore, LaneScoreForTeam());
		}
	}
}