using System.ComponentModel.DataAnnotations;
using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class CreatePlayerViewModel
    {
        public CreatePlayerViewModel()
        {
            Name = string.Empty;
            Email = string.Empty;
            Status = Player.Status.Active;
        }

        public CreatePlayerViewModel(Player player)
        {
            Name = player.Name;
            Email = player.Email;
            Status = player.PlayerStatus;
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public Player.Status Status { get; set; }
    }
}