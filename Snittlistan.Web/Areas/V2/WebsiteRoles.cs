#nullable enable

namespace Snittlistan.Web.Areas.V2;

public static class WebsiteRoles
{
    // roles
    private static readonly List<WebsiteRole> Roles = new();

    public static void Initialize()
    {
        Roles.Add(new WebsiteRole(Absence.View, "Visa frånvaro", RoleLevel.Player));
        Roles.Add(new WebsiteRole(Activity.Manage, "Aktiviteter (banledare och klubbmästare)", RoleLevel.User));
        Roles.Add(new WebsiteRole(EliteMedals.EditMedals, "Elitmärken", RoleLevel.User));
        Roles.Add(new WebsiteRole(Player.Admin, "Administratör (fullständig access)", RoleLevel.User));
        Roles.Add(new WebsiteRole(Player.ShowEmailAddresses, "Visa e-postadresser", RoleLevel.Player));
        Roles.Add(new WebsiteRole(Uk.UkTasks, "UK-rollen (uttagningar och frånvaro)", RoleLevel.User));
    }

    public static class Absence
    {
        public const string View = "Absence.View";
    }

    public static class EliteMedals
    {
        public const string EditMedals = "EliteMedals.EditMedals";
    }

    public static class Player
    {
        public const string Admin = "Player.Admin";

        public const string ShowEmailAddresses = "Player.ShowEmailAddresses";
    }

    public static class Uk
    {
        public const string UkTasks = "Uk.UkTasks";
    }

    public static class Activity
    {
        public const string Manage = "Activity.Manage";
    }

    // groups
    public static WebsiteRole[] PlayerGroup()
    {
        return Roles.Where(x => x.RoleLevel <= RoleLevel.Player).ToArray();
    }

    public static WebsiteRole[] UserGroup()
    {
        return Roles.Where(x => x.RoleLevel <= RoleLevel.User).ToArray();
    }

    public static WebsiteRole[] AdminGroup()
    {
        return Roles.Where(x => x.RoleLevel <= RoleLevel.Admin).ToArray();
    }

    public static IReadOnlyDictionary<string, WebsiteRole> ToDict(this WebsiteRole[] roles)
    {
        return roles.ToDictionary(x => x.Name);
    }

    public enum RoleLevel
    {
        Player,
        User,
        Admin
    }

    public class WebsiteRole
    {
        public WebsiteRole(string name, string description, RoleLevel roleLevel)
        {
            Name = name;
            Description = description;
            RoleLevel = roleLevel;
        }

        public string Name { get; }

        public string Description { get; }

        public RoleLevel RoleLevel { get; }
    }
}
