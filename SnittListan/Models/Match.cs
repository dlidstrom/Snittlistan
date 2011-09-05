using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace SnittListan.Models
{
	public class Match
	{
		public Match()
		{
			Place = string.Empty;
			Date = DateTime.Now.Date;
		}

		public Match(
			string place,
			DateTime date,
			Team homeTeam,
			Team awayTeam,
			int bitsMatchId)
		{
			Place = place;
			Date = date;
			HomeTeam = homeTeam;
			AwayTeam = awayTeam;
			BitsMatchId = bitsMatchId;
		}

		public int Id { get; set; }
		public string Place { get; private set; }
		public DateTime Date { get; private set; }
		public Team HomeTeam { get; private set; }
		public Team AwayTeam { get; private set; }
		public int BitsMatchId { get; private set; }

		public int PinscoreForTeam(string team, int serie)
		{
			return 0; // Games.Where(g => g.Team == team && g.SerieNumber == serie).Sum(g => g.PinScore);
		}

		public int LaneScoreForTeam(string team)
		{
			return 0; // Games.Where(g => g.Team == team).Sum(g => g.LaneScore) / 2;
		}

		public int LaneScoreForTeam(string team, int serie)
		{
			return 0; // Games.Where(g => g.Team == team && g.SerieNumber == serie).Sum(g => g.LaneScore) / 2;
		}

		public string FormattedLaneScore()
		{
			return string.Empty;
			////if (HomeGame)
			////    return string.Format("{0}-{1}", LaneScoreForTeam(), OppTeamLaneScore);
			////else
			////    return string.Format("{0}-{1}", OppTeamLaneScore, LaneScoreForTeam());
		}
	}
}