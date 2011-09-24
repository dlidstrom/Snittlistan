using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Snittlistan.ViewModels
{
	public class MatchViewModel
	{
		public MatchDetails Match { get; set; }

		public TeamViewModel HomeTeam { get; set; }

		public TeamViewModel AwayTeam { get; set; }

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
	}
}