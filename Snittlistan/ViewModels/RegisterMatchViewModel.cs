using System;
using System.ComponentModel.DataAnnotations;

namespace Snittlistan.ViewModels
{
	/// <summary>
	/// Represents the view used to register a match.
	/// </summary>
	public class RegisterMatchViewModel
	{
		/// <summary>
		/// Initializes a new instance of the RegisterMatchViewModel class.
		/// </summary>
		public RegisterMatchViewModel()
		{
			Date = DateTime.Now;
			Location = string.Empty;
			HomeTeam = new HomeTeamViewModel();
			AwayTeam = new AwayTeamViewModel();
		}

		/// <summary>
		/// Gets or sets the match date.
		/// </summary>
		[Required, Display(Name = "Datum"), DataType(DataType.Date)]
		public DateTime Date { get; set; }

		/// <summary>
		/// Gets or sets the match location.
		/// </summary>
		[Required, Display(Name = "Plats")]
		public string Location { get; set; }

		/// <summary>
		/// Gets or sets the bits match id.
		/// </summary>
		[Required, Display(Name = "BITS MatchId")]
		public int BitsMatchId { get; set; }

		/// <summary>
		/// Gets or sets the home team.
		/// </summary>
		[Display(Name = "Hemmalag")]
		public HomeTeamViewModel HomeTeam { get; set; }

		/// <summary>
		/// Gets or sets the away team.
		/// </summary>
		[Display(Name = "Bortalag")]
		public AwayTeamViewModel AwayTeam { get; set; }
	}
}