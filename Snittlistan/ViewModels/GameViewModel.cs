using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SnittListan.ViewModels
{
	public class GameViewModel
	{
		[HiddenInput]
		public int MatchId { get; set; }

		[Required, Display(Name = "Serie"), Range(1, 4)]
		public int Serie { get; set; }

		[Required, Display(Name = "Bord"), Range(1, 4)]
		public int Table { get; set; }

		[Required, Display(Name = "Spelare")]
		public string Player { get; set; }

		[Required, Display(Name = "Kägelpoäng"), Range(0, 300)]
		public int Pins { get; set; }

		[Required, Display(Name = "Banpoäng")]
		public int Score { get; set; }
	}
}