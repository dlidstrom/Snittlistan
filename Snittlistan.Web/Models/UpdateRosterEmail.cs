#nullable enable

namespace Snittlistan.Web.Models
{
    using Snittlistan.Web.Areas.V2.Domain;

    public class UpdateRosterEmail : EmailBase
    {
        public UpdateRosterEmail(
            string to,
            FormattedAuditLog formattedAuditLog,
            string[] players,
            string? teamLeader)
            : base("UpdateRoster", OwnerEmail, "Uttagning har uppdaterats")
        {
            To = to;
            FormattedAuditLog = formattedAuditLog;
            Players = players;
            TeamLeader = teamLeader;
        }

        public string To { get; }
        public FormattedAuditLog FormattedAuditLog { get; }
        public string[] Players { get; }
        public string? TeamLeader { get; }
    }
}
