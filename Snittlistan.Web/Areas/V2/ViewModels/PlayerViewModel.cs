using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class PlayerViewModel
    {
        public PlayerViewModel(Player player)
        {
            Id = player.Id;
            Name = player.Name;
            Email = player.Email;
            Status = player.PlayerStatus;
            switch (player.PlayerStatus)
            {
                case Player.Status.Active:
                    StatusText = "Aktiv";
                    break;

                case Player.Status.Inactive:
                    StatusText = "Inaktiv";
                    break;

                case Player.Status.Supporter:
                    StatusText = "Supporter";
                    break;
            }
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public Player.Status Status { get; set; }

        public string StatusText { get; set; }
    }
}