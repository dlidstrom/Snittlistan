#nullable enable

namespace Snittlistan.Web.Areas.V2.Domain;

public class RosterState
{
    public RosterState(string[] players, string[] acceptedPlayers, string? teamLeader)
    {
        Players = players;
        AcceptedPlayers = acceptedPlayers;
        TeamLeader = teamLeader;
    }

    public string[] Players { get; }

    public string[] AcceptedPlayers { get; }

    public string? TeamLeader { get; }
}
