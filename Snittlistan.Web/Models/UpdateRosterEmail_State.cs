#nullable enable

using Postal;
using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Web.Models;

public class UpdateRosterEmail_State : EmailState
{
    public UpdateRosterEmail_State(
        string playerEmail,
        string name,
        FormattedAuditLog formattedAuditLog,
        string[] players,
        string? teamLeader,
        string replyToEmail,
        int season,
        int turn,
        Uri rosterLink,
        Uri userProfileLink)
        : base(OwnerEmail, playerEmail, OwnerEmail, "Uttagning har uppdaterats")
    {
        PlayerEmail = playerEmail;
        Name = name;
        FormattedAuditLog = formattedAuditLog;
        Players = players;
        TeamLeader = teamLeader;
        ReplyToEmail = replyToEmail;
        Season = season;
        Turn = turn;
        RosterLink = rosterLink;
        UserProfileLink = userProfileLink;
    }

    public string PlayerEmail { get; }

    public string Name { get; }

    public FormattedAuditLog FormattedAuditLog { get; }

    public string[] Players { get; }

    public string? TeamLeader { get; }

    public string ReplyToEmail { get; }

    public int Season { get; }

    public int Turn { get; }

    public Uri RosterLink { get; }

    public Uri UserProfileLink { get; }

    public override Email CreateEmail()
    {
        return new UpdateRosterEmail(
            PlayerEmail,
            Name,
            FormattedAuditLog,
            Players,
            TeamLeader,
            ReplyToEmail,
            Season,
            Turn,
            RosterLink,
            UserProfileLink);
    }
}
