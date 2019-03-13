namespace Snittlistan.Web.Areas.V2.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Snittlistan.Web.Areas.V2.Domain;

    public class PlayerViewModel
    {
        public PlayerViewModel(
            Player player,
            IReadOnlyDictionary<string, WebsiteRoles.WebsiteRole> rolesDict)
        {
            Id = player.Id;
            Name = player.Name;
            Nickname = player.Nickname;
            Email = player.Email;
            Status = player.PlayerStatus;
            Roles = player.Roles
                          .Where(rolesDict.ContainsKey)
                          .Select(x => rolesDict[x].Description)
                          .OrderBy(x => x)
                          .ToArray();
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