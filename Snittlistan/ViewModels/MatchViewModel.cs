using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Snittlistan.ViewModels
{
	public class MatchViewModel
	{
		public MatchInfo Info { get; set; }

		public Game[] HomeTeamGames { get; set; }

		public Game[] AwayTeamGames { get; set; }

		public class MatchInfo
		{
			public MatchInfo()
			{
				Date = DateTime.Now.Date;
			}

			[HiddenInput]
			public int Id { get; set; }

			[Required, Display(Name = "BITS MatchId")]
			public int BitsMatchId { get; set; }

			[Required, Display(Name = "Plats")]
			public string Place { get; set; }

			[Required, Display(Name = "Datum"), DataType(DataType.Date)]
			public DateTime Date { get; set; }

			[Required, Display(Name = "Hemmalag")]
			public string HomeTeam { get; set; }

			[Required, Display(Name = "Banpoäng"), Range(0, 20)]
			public int HomeTeamScore { get; set; }

			[Required, Display(Name = "Bortalag")]
			public string AwayTeam { get; set; }

			[Required, Display(Name = "Banpoäng"), Range(0, 20)]
			public int AwayTeamScore { get; set; }
		}

		public class Game
		{
			[Display(Name = "Spelare")]
			public string Player { get; set; }

			[Display(Name = "Kägelpoäng")]
			public int Pins { get; set; }

			[Display(Name = "Banpoäng")]
			public int Score { get; set; }

			[Display(Name = "X")]
			public int Strikes { get; set; }

			[Display(Name = "Miss")]
			public int Misses { get; set; }

			[Display(Name = "9-")]
			public int OnePinMisses { get; set; }

			[Display(Name = "Hål")]
			public int Splits { get; set; }
		}
	}
}