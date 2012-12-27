namespace Snittlistan.Web.Areas.V2.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class CreateUserViewModel
    {
        public CreateUserViewModel()
        {
            Email = ConfirmEmail = string.Empty;
        }

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, DataType(DataType.EmailAddress), Compare("Email")]
        public string ConfirmEmail { get; set; }
    }
}