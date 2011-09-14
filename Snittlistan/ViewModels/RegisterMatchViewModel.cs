using System;
using System.ComponentModel.DataAnnotations;

namespace Snittlistan.ViewModels
{
	public class RegisterMatchViewModel
	{
		public RegisterMatchViewModel()
		{
			Date = DateTime.Now;
		}

		[Required, Display(Name = "Datum"), DataType(DataType.Date)]
		public DateTime Date { get; set; }

		[Required, Display(Name = "Plats")]
		public string Place { get; set; }

		[Required, Display(Name = "BITS MatchId")]
		public int BitsMatchId { get; set; }

		[Required, Display(Name = "Hemmalag")]
		public string HomeTeamName { get; set; }

		[Required, Display(Name = "Banpoäng"), Range(0, 20)]
		public int HomeTeamScore { get; set; }

		[Required, Display(Name = "Bortalag")]
		public string AwayTeamName { get; set; }

		[Required, Display(Name = "Banpoäng"), Range(0, 20)]
		public int AwayTeamScore { get; set; }
	}
}