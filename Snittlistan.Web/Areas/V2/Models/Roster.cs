namespace Snittlistan.Web.Areas.V2.Models
{
    using System;
    using System.Collections.Generic;

    public class Roster
    {
        public Roster(int season, int turn, string team, string location, string opponent, DateTime date)
        {
            this.Season = season;
            this.Turn = turn;
            this.Team = team;
            this.Location = location;
            this.Opponent = opponent;
            this.Date = date;
            this.Players = new List<string>();
        }

        public int Id { get; set; }

        public int Season { get; set; }

        public int Turn { get; set; }

        public string Team { get; set; }

        public string Location { get; set; }

        public string Opponent { get; set; }

        public DateTime Date { get; set; }

        public bool Preliminary { get; set; }

        public List<string> Players { get; set; }

        public string MatchResultId { get; set; }
    }
}