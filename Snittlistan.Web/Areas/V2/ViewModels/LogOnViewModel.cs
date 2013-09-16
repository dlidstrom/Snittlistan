using System.ComponentModel.DataAnnotations;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class LogOnViewModel
    {
        [Required(ErrorMessage = "Ange e-postadress")]
        [DataType(DataType.EmailAddress), Display(Name = "E-postadress")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Ange lösenord")]
        [DataType(DataType.Password), Display(Name = "Lösenord")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}