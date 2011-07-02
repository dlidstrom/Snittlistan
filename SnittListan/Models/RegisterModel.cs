using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SnittListan.Models
{
	public class RegisterModel
	{
		[Required(ErrorMessage = "Ange ditt förnamn")]
		[Display(Name = "Förnamn")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Ange ditt efternamn")]
		[Display(Name = "Efternamn")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Ange din e-postadress")]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "E-postadress")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Ange din e-postadress igen")]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "Upprepa e-postadressen")]
		[Compare("Email", ErrorMessage = "E-postadresserna är olika")]
		public string ConfirmEmail { get; set; }

		[Required(ErrorMessage = "Ange ett lösenord")]
		[StringLength(100, ErrorMessage = "Ditt lösenord måste vara minst {2} tecken långt.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Lösenord")]
		public string Password { get; set; }
	}
}