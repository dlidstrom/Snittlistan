namespace Snittlistan.Web.Areas.V1.ViewModels.Admin
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class EditUserViewModel
    {
        [HiddenInput]
        public string Id { get; set; }

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

        [Required(ErrorMessage = "Ange ett lösenord")]
        [StringLength(100, ErrorMessage = "Ditt lösenord måste vara minst {2} tecken långt.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }

        [Display(Name = "Aktiv")]
        public bool IsActive { get; set; }
    }
}