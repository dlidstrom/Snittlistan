#nullable enable

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class SetPasswordViewModel
    {
        [HiddenInput]
        public string ActivationKey { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required, System.ComponentModel.DataAnnotations.Compare("Password")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
