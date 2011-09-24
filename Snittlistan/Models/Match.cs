using System;
using System.Collections.Generic;

namespace Snittlistan.Models
{
	public class Match
	{
		/// <summary>
		/// Initializes a new instance of the Match class.
		/// </summary>
		/// <param name="location">Match location.</param>
		/// <param name="date">Match date.</param>
		/// <param name="bitsMatchId">BITS match id.</param>
		/// <param name="homeTeam">Home team.</param>
		/// <param name="awayTeam">Away team.</param>
		public Match(
			string location,
			DateTime date,
			int bitsMatchId,
			Team homeTeam,
			Team awayTeam)
		{
			Location = location;
			Date = date;
			BitsMatchId = bitsMatchId;
			HomeTeam = homeTeam;
			AwayTeam = awayTeam;
		}

		public int Id { get; set; }
		public string Location { get; set; }
		public DateTime Date { get; set; }
		public Team HomeTeam { get; private set; }
		public Team AwayTeam { get; private set; }
		public int BitsMatchId { get; set; }
	}
}