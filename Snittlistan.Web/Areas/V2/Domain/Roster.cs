using System;
using System.Collections.Generic;

namespace Snittlistan.Web.Areas.V2.Domain
{
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
            Players = new List<string>();
        }

        public string Id { get; set; }

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