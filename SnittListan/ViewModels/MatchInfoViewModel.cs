using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SnittListan.ViewModels
{
	public class MatchInfoViewModel
	{
		public MatchInfoViewModel()
		{
			Date = DateTime.Now.Date;
		}

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
}