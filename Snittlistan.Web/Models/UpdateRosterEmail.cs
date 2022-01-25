#nullable enable

using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Web.Models;

public class UpdateRosterEmail : EmailBase
{
    private readonly UpdateRosterEmail_State _state;

    public UpdateRosterEmail(
        string playerEmail,
        FormattedAuditLog formattedAuditLog,
        string[] players,
        string? teamLeader)
        : base("UpdateRoster")
    {
        _state = new(
            playerEmail,
            formattedAuditLog,
            players,
            teamLeader);
    }

    public string PlayerEmail => _state.PlayerEmail;

    public FormattedAuditLog FormattedAuditLog => _state.FormattedAuditLog;

    public string[] Players => _state.Players;

    public string? TeamLeader => _state.TeamLeader;

    public override EmailState State => _state;
}
