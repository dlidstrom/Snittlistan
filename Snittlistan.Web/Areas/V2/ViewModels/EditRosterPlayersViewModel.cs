namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class EditRosterPlayersViewModel
    {
        public int Id { get; set; }

        public RosterViewModel Roster { get; set; }

        public PlayerViewModel[] AvailablePlayers { get; set; }
    }
}