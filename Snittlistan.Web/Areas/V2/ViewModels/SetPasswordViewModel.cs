using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using JetBrains.Annotations;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    [UsedImplicitly]
    public class SetPasswordViewModel
    {
        [HiddenInput]
        public string ActivationKey { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, System.ComponentModel.DataAnnotations.Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}