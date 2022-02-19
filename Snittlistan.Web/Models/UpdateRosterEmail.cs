#nullable enable

using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Web.Models;

public class UpdateRosterEmail : EmailBase
{
    private readonly UpdateRosterEmail_State _state;

    public UpdateRosterEmail(
        string playerEmail,
        string name,
        FormattedAuditLog formattedAuditLog,
        string[] players,
        string? teamLeader,
        string replyToEmail,
        int season,
        int turn,
        Uri rosterLink)
        : base("UpdateRoster")
    {
        _state = new(
            playerEmail,
            name,
            formattedAuditLog,
            players,
            teamLeader,
            replyToEmail,
            season,
            turn,
            rosterLink);
    }

    public string PlayerEmail => _state.PlayerEmail;

    public string Name => _state.Name;

    public FormattedAuditLog FormattedAuditLog => _state.FormattedAuditLog;

    public string[] Players => _state.Players;

    public string? TeamLeader => _state.TeamLeader;

    public string ReplyToEmail => _state.ReplyToEmail;

    public int Season => _state.Season;

    public int Turn => _state.Turn;

    public Uri RosterLink => _state.RosterLink;

    public override EmailState State => _state;
}
