using Snittlistan.Web.Areas.V2.Domain;

#nullable enable

namespace Snittlistan.Web.Areas.V2.ViewModels;
public class PlayerViewModel
{
    public PlayerViewModel(
        Player player,
        IReadOnlyDictionary<string, WebsiteRoles.WebsiteRole> rolesDict)
    {
        Id = player.Id;
        Name = player.Name;
        Nickname = player.Nickname!;
        Email = player.Email;
        Status = player.PlayerStatus;
        Roles = player.Roles
                      .Where(rolesDict.ContainsKey)
                      .Select(x => rolesDict[x].Description)
                      .OrderBy(x => x)
                      .ToArray();
        StatusText = player.PlayerStatus switch
        {
            Player.Status.Active => "Aktiv",
            Player.Status.Inactive => "Inaktiv",
            Player.Status.Supporter => "Supporter",
            _ => throw new ArgumentOutOfRangeException(),
        };
        LicenseNumber = player.PlayerItem?.LicNbr ?? string.Empty;
    }

    public string Id { get; }

    public string Name { get; }

    public string Email { get; }

    public Player.Status Status { get; }

    public string StatusText { get; }

    public string Nickname { get; }

    public string[] Roles { get; }

    public string LicenseNumber { get; }
}
