
using Snittlistan.Web.Areas.V2.Domain;

#nullable enable

namespace Snittlistan.Web.Areas.V2.ViewModels;
public class RosterViewModel
{
    public RosterViewModel()
    {
        Players = new List<PlayerItem>();
    }

    public RosterViewModel(
        Roster roster,
        PlayerItem? teamLeader,
        List<PlayerItem> players)
    {
        TeamLeader = teamLeader;
        Players = players;
        Season = roster.Season;
        Turn = roster.Turn;
        IsFourPlayer = roster.IsFourPlayer;
        Preliminary = roster.Preliminary;
        Header = new RosterHeaderViewModel(
            roster.Id,
            roster.Team,
            roster.TeamLevel,
            roster.Location,
            roster.Opponent,
            roster.Date,
            roster.OilPattern,
            roster.MatchResultId,
            roster.MatchTimeChanged);
    }

    public RosterHeaderViewModel? Header { get; set; }

    public int Season { get; set; }

    public int Turn { get; set; }

    public bool IsFourPlayer { get; set; }

    public bool Preliminary { get; set; }

    public List<PlayerItem> Players { get; set; }

    public PlayerItem? TeamLeader { get; set; }

    public class PlayerItem
    {
        public PlayerItem(string playerId, string playerName, bool accepted)
        {
            PlayerId = playerId;
            PlayerName = playerName;
            Accepted = accepted;
        }

        public string PlayerId { get; }
        public string PlayerName { get; }
        public bool Accepted { get; }
    }
}
