
using Snittlistan.Web.Areas.V2.Domain;

#nullable enable

namespace Snittlistan.Web.Models;
public class UpdateRosterEmail : EmailBase
{
    public UpdateRosterEmail(
        string playerEmail,
        FormattedAuditLog formattedAuditLog,
        string[] players,
        string? teamLeader)
        : base("UpdateRoster", OwnerEmail, "Uttagning har uppdaterats")
    {
        PlayerEmail = playerEmail;
        FormattedAuditLog = formattedAuditLog;
        Players = players;
        TeamLeader = teamLeader;
    }

    public string PlayerEmail { get; }
    public FormattedAuditLog FormattedAuditLog { get; }
    public string[] Players { get; }
    public string? TeamLeader { get; }
}
