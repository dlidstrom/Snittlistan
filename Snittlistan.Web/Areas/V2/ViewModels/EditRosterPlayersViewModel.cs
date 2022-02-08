#nullable enable

using Snittlistan.Web.Infrastructure.Database;

namespace Snittlistan.Web.Areas.V2.ViewModels;

public class EditRosterPlayersViewModel
{
    public RosterViewModel? RosterViewModel { get; set; }

    public PlayerViewModel[]? AvailablePlayers { get; set; }

    public TenantFeatures? Features { get; set; } = null!;
}
