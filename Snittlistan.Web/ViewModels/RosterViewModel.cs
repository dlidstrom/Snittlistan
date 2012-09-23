namespace Snittlistan.Web.ViewModels
{
    public class RosterViewModel
    {
        public string Team { get; set; }

        public string Location { get; set; }

        public string Opponent { get; set; }

        public string Date { get; set; }

        public char TeamLevel
        {
            get
            {
                return Team[Team.Length - 1];
            }
        }
    }
}