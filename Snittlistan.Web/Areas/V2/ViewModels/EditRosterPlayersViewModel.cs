namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class EditRosterPlayersViewModel
    {
        public string Id { get; set; }

        public RosterViewModel Roster { get; set; }

        public PlayerViewModel[] AvailablePlayers { get; set; }

        public bool Preliminary { get; set; }
    }
}