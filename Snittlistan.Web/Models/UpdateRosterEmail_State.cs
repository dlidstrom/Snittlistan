#nullable enable

using Postal;
using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Web.Models;

public class UpdateRosterEmail_State : EmailState
{
    public UpdateRosterEmail_State(
        string playerEmail,
        FormattedAuditLog formattedAuditLog,
        string[] players,
        string? teamLeader,
        string replyToEmail,
        int season,
        int turn)
        : base(OwnerEmail, playerEmail, OwnerEmail, "Uttagning har uppdaterats")
    {
        PlayerEmail = playerEmail;
        FormattedAuditLog = formattedAuditLog;
        Players = players;
        TeamLeader = teamLeader;
        ReplyToEmail = replyToEmail;
        Season = season;
        Turn = turn;
    }

    public string PlayerEmail { get; }

    public FormattedAuditLog FormattedAuditLog { get; }

    public string[] Players { get; }

    public string? TeamLeader { get; }

    public string ReplyToEmail { get; }

    public int Season { get; }

    public int Turn { get; }

    public override Email CreateEmail()
    {
        return new UpdateRosterEmail(
            PlayerEmail,
            FormattedAuditLog,
            Players,
            TeamLeader,
            ReplyToEmail,
            Season,
            Turn);
    }
}
