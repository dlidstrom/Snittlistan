namespace Snittlistan.Web.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;

    public class LogOnViewModel
    {
        [Required(ErrorMessage = "Ange e-postadress")]
        [DataType(DataType.EmailAddress), Display(Name = "E-postadress")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Ange lösenord")]
        [DataType(DataType.Password), Display(Name = "Lösenord")]
        public string Password { get; set; }

        [Display(Name = "Kom ihåg mig?")]
        public bool RememberMe { get; set; }
    }
}