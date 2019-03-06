using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    using System.Linq;

    public class PlayerViewModel
    {
        public PlayerViewModel(Player player)
        {
            Id = player.Id;
            Name = player.Name;
            Nickname = player.Nickname;
            Email = player.Email;
            Status = player.PlayerStatus;
            var rolesDict = WebsiteRoles.UserGroup().ToDictionary(x => x.Name);
            Roles = player.Roles.Select(x => rolesDict[x].Description).OrderBy(x => x).ToArray();
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

        public string Id { get; }

        public string Name { get; }

        public string Email { get; }

        public Player.Status Status { get; }

        public string StatusText { get; }

        public string Nickname { get; }

        public string[] Roles { get; }
    }
}