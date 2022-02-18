#nullable enable

namespace Snittlistan.Web.Areas.V2.ViewModels;

public class EditRosterPlayersViewModel
{
    public RosterViewModel? RosterViewModel { get; set; }

    public PlayerViewModel[]? AvailablePlayers { get; set; }

    public bool RosterMailEnabled { get; set; }
}
