namespace Snittlistan.Web.Areas.V2.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class VerifyBitsViewModel
    {
        [Required]
        public int BitsMatchId { get; set; }
    }
}