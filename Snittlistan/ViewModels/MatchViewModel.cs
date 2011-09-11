using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Snittlistan.ViewModels
{
	public class MatchViewModel
	{
		public MatchDetails Match { get; set; }

		public Team HomeTeam { get; set; }

		public Team AwayTeam { get; set; }

		public class MatchDetails
		{
			public MatchDetails()
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
		}

		public class Team
		{
			[HiddenInput]
			public int Id { get; set; }

			[Required, Display(Name = "Namn")]
			public string Name { get; set; }

			[Required, Display(Name = "Banpoäng"), Range(0, 20)]
			public int Score { get; set; }

			public IList<Game> Games { get; set; }
		}

		public class Game
		{
			[Required, Display(Name = "Spelare")]
			public string Player { get; set; }

			[Required, Display(Name = "Serie")]
			public int Serie { get; set; }

			[Required, Display(Name = "Bord")]
			public int Table { get; set; }

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