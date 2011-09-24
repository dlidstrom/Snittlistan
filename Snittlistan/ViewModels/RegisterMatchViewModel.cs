using System;
using System.ComponentModel.DataAnnotations;

namespace Snittlistan.ViewModels
{
	public class RegisterMatchViewModel
	{
		public RegisterMatchViewModel()
		{
			Date = DateTime.Now;
			Location = string.Empty;
			HomeTeam = new TeamViewModel();
			AwayTeam = new TeamViewModel();
		}

		[Required, Display(Name = "Datum"), DataType(DataType.Date)]
		public DateTime Date { get; set; }

		[Required, Display(Name = "Plats")]
		public string Location { get; set; }

		[Required, Display(Name = "BITS MatchId")]
		public int BitsMatchId { get; set; }

		[Display(Name = "Hemmalag")]
		public TeamViewModel HomeTeam { get; set; }

		[Display(Name = "Bortalag")]
		public TeamViewModel AwayTeam { get; set; }
	}
}