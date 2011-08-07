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
			List<Game> games)
		{
			Id = id;
			Place = place;
			Date = date;
			HomeGame = homeGame;
			HomeTeam = homeTeam;
			OppTeam = oppTeam;
			OppTeamLaneScore = oppTeamLaneScore;
			Games = games;
		}

		public int Id { get; private set; }
		public string Place { get; private set; }
		public DateTime Date { get; private set; }
		public bool HomeGame { get; private set; }
		public string HomeTeam { get; private set; }
		public string OppTeam { get; private set; }
		public int OppTeamLaneScore { get; private set; }
		public List<Game> Games { get; private set; }

		[JsonIgnore]
		public int NumberOfSeries
		{
			get
			{
				return Games.Max(g => g.SerieNumber);
			}
		}

		public int PinScoreForTeam()
		{
			return Games.Sum(g => g.PinScore);
		}

		public int PinscoreForTeam(int serie)
		{
			return Games.Where(g => g.SerieNumber == serie).Sum(g => g.PinScore);
		}

		public int PinscoreForPlayer(string player)
		{
			return Games.Where(g => g.Player == player).Sum(g => g.PinScore);
		}

		public int LaneScoreForTeam()
		{
			return Games.Sum(g => g.LaneScore) / 2;
		}

		public int LaneScoreForTeam(int serie)
		{
			return Games.Where(g => g.SerieNumber == serie).Sum(g => g.LaneScore) / 2;
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