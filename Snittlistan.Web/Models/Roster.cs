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

        public int Season { get; private set; }

        public int Turn { get; private set; }

        public string Team { get; private set; }

        public string Location { get; private set; }

        public string Opponent { get; private set; }

        public DateTime Date { get; private set; }
    }
}