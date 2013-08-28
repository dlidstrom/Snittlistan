using System.ComponentModel.DataAnnotations;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class CreatePlayerViewModel
    {
        public CreatePlayerViewModel()
        {
            Name = string.Empty;
            Email = string.Empty;
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public bool IsSupporter { get; set; }
    }
}