using System.ComponentModel.DataAnnotations;

namespace SnittListan.Models
{
	public class LogOnModel
	{
		[Required]
		[Display(Name = "E-postadress")]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Lösenord")]
		public string Password { get; set; }

		[Display(Name = "Kom ihåg mig?")]
		public bool RememberMe { get; set; }
	}
}