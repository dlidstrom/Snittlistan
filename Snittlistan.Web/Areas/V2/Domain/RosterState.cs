#nullable enable

namespace Snittlistan.Web.Areas.V2.Domain;

public class RosterState
{
    public RosterState(string[] players, string[] acceptedPlayers)
    {
        Players = players;
        AcceptedPlayers = acceptedPlayers;
    }

    public string[] Players { get; }

    public string[] AcceptedPlayers { get; }
}
