using System;
using System.ComponentModel.DataAnnotations;

namespace SnittListan.ViewModels
{
	public class MatchViewModel
	{
		public string Id { get; set; }

		[Required, Display(Name = "Plats")]
		public string Place { get; set; }

		[Required, Display(Name = "Datum"), DataType(DataType.Date)]
		public DateTime Date { get; set; }

		[Required, Display(Name = "Hemmalag")]
		public string HomeTeam { get; set; }

		[Required, Display(Name = "Banpoäng")]
		public int HomeTeamLaneScore { get; set; }

		[Required, Display(Name = "Bortalag")]
		public string OppTeam { get; set; }

		[Required, Display(Name = "Banpoäng")]
		public int OppTeamLaneScore { get; set; }

		public Game[] Games { get; set; }

		public class Game
		{
			public string Player { get; set; }
			public int PinScore { get; set; }
			public int LaneScore { get; set; }
		}
	}
}