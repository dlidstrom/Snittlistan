namespace Snittlistan.Web.Areas.V2
{
    using System.Collections.Generic;
    using System.Linq;

    public static class WebsiteRoles
    {
        // roles
        private static readonly List<WebsiteRole> Roles = new List<WebsiteRole>();

        public static void Initialize()
        {
            var absenceView =
                new WebsiteRole(Absence.View, RoleLevel.Player);

            var editAllRole =
                new WebsiteRole(Absence.EditAll, RoleLevel.User);

            var editMedalsRole =
                new WebsiteRole(EliteMedals.EditMedals, RoleLevel.User);

            var generateReportRole =
                new WebsiteRole(EliteMedals.GenerateReport, RoleLevel.User);

            var addManualResultRole =
                new WebsiteRole(MatchResult.AddManualResult, RoleLevel.User);

            var editPlayerRole =
                new WebsiteRole(Player.EditPlayer, RoleLevel.User);

            var sendEmailToAllRole =
                new WebsiteRole(Player.SendEmailToAll, RoleLevel.Player);

            var showEmailAddressesRole =
                new WebsiteRole(Player.ShowEmailAddresses, RoleLevel.Player);

            var showInactiveRole =
                new WebsiteRole(Player.ShowInactive, RoleLevel.User);

            var addRosterRole =
                new WebsiteRole(Roster.AddRoster, RoleLevel.User);
            var editPlayersRole =
                new WebsiteRole(Roster.EditPlayers, RoleLevel.User);
            var viewPreliminaryRole =
                new WebsiteRole(Roster.ViewPreliminary, RoleLevel.User);
            var editRosterRole =
                new WebsiteRole(Roster.EditRoster, RoleLevel.User);
        }

        public static class Absence
        {
            public const string View = "Absence.View";

            public const string EditAll = "Absence.EditAll";
        }

        public static class EliteMedals
        {
            public const string EditMedals = "EliteMedals.EditMedals";

            public const string GenerateReport = "EliteMedals.GenerateReport";
        }

        public static class MatchResult
        {
            public const string AddManualResult = "MatchResult.AddManualResult";
        }

        public static class Player
        {
            public const string EditPlayer = "Player.EditPlayer";

            public const string SendEmailToAll = "Player.SendEmailToAll";

            public const string ShowEmailAddresses = "Player.ShowEmailAddresses";

            public const string ShowInactive = "Player.ShowInactive";
        }

        public static class Roster
        {
            public const string AddRoster = "Roster.AddRoster";

            public const string EditPlayers = "Roster.EditPlayers";

            public const string ViewPreliminary = "Roster.ViewPreliminary";

            public const string EditRoster = "Roster.EditRoster";
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

        public enum RoleLevel
        {
            Player,
            User,
            Admin
        }

        public class WebsiteRole
        {
            public WebsiteRole(string name, RoleLevel roleLevel)
            {
                Name = name;
                RoleLevel = roleLevel;
                Roles.Add(this);
            }

            public string Name { get; }

            public RoleLevel RoleLevel { get; }
        }
    }
}
