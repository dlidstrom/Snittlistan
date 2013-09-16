namespace Snittlistan.Web.Areas.V1.Models
{
    public class TurnModel
    {
        public TurnModel(int turn, string team, string opponent, string location, string date, string time)
        {
            Turn = turn;
            Team = team;
            Opponent = opponent;
            Location = location;
            Date = date;
            Time = time;
        }

        public int Id { get; set; }

        public int Turn { get; private set; }

        public string Team { get; private set; }

        public string Opponent { get; private set; }

        public string Location { get; private set; }

        public string Date { get; private set; }

        public string Time { get; private set; }
    }
}