namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class ViewTurnViewModel
    {
        public int Id { get; set; }

        public int Season { get; set; }

        public RosterViewModel[] Rosters { get; set; }

        public PlayerViewModel[] AvailablePlayers { get; set; }
    }
}