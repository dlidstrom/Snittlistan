using System;

namespace Snittlistan.Models
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
			int bitsMatchId,
			Team homeTeam,
			Team awayTeam)
		{
			Place = place;
			Date = date;
			BitsMatchId = bitsMatchId;
			HomeTeam = homeTeam;
			AwayTeam = awayTeam;
		}

		public int Id { get; set; }
		public string Place { get; set; }
		public DateTime Date { get; set; }
		public Team HomeTeam { get; private set; }
		public Team AwayTeam { get; private set; }
		public int BitsMatchId { get; set; }

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