using System.ComponentModel.DataAnnotations;

namespace Snittlistan.ViewModels
{
	public class TeamSummaryViewModel
	{
		[Display(Name = "Namn")]
		public string Name { get; set; }

		[Display(Name = "Banpoäng")]
		public int Score { get; set; }
	}
}