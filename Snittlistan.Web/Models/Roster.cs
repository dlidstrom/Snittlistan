namespace Snittlistan.Web.Models
{
    using System;

    public class Roster
    {
        public Roster(int season, int turn, string team, string location, string opponent, DateTime date)
        {
            Season = season;
            Turn = turn;
            Team = team;
            Location = location;
            Opponent = opponent;
            Date = date;
        }

        public int Id { get; set; }

        public int Season { get; set; }

        public int Turn { get; set; }

        public string Team { get; set; }

        public string Location { get; set; }

        public string Opponent { get; set; }

        public DateTime Date { get; set; }
    }
}