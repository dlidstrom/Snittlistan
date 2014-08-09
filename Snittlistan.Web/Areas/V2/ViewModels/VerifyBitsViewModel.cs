using System.ComponentModel.DataAnnotations;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class VerifyBitsViewModel
    {
        [Required]
        public int BitsMatchId { get; set; }
    }
}