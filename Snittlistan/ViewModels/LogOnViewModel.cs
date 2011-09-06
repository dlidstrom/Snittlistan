using System.ComponentModel.DataAnnotations;

namespace SnittListan.ViewModels
{
	public class LogOnViewModel
	{
		[Required, DataType(DataType.EmailAddress), Display(Name = "E-postadress")]
		public string Email { get; set; }

		[Required, DataType(DataType.Password), Display(Name = "Lösenord")]
		public string Password { get; set; }

		[Display(Name = "Kom ihåg mig?")]
		public bool RememberMe { get; set; }
	}
}