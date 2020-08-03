namespace Snittlistan.Web.Areas.V2.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class RegisterBitsVerifyModel
    {
        [Required]
        public string RosterId { get; set; }

        [HiddenInput]
        public int Season { get; set; }
    }
}