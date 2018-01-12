using System;
using System.Collections.Generic;

namespace Snittlistan.Web.Areas.V2.Domain
{
    public class Roster
    {
        private string teamLevel;

        public Roster(
            int season,
            int turn,
            int bitsMatchId,
            string team,
            string teamLevel,
            string location,
            string opponent,
            DateTime date,
            bool isFourPlayer)
        {
            Season = season;
            Turn = turn;
            BitsMatchId = bitsMatchId;
            Team = team;
            TeamLevel = teamLevel ?? Team.Substring(team.Length - 1);
            Location = location;
            Opponent = opponent;
            Date = date;
            IsFourPlayer = isFourPlayer;
            Players = new List<string>();
        }

        public string Id { get; set; }

        public int Season { get; set; }

        public int Turn { get; set; }

        public int BitsMatchId { get; set; }

        public string Team { get; set; }

        public string TeamLevel
        {
            get { return teamLevel; }
            set { teamLevel = value.Trim(); }
        }

        public string Location { get; set; }

        public string Opponent { get; set; }

        public DateTime Date { get; set; }

        public bool IsFourPlayer { get; set; }

        public bool Preliminary { get; set; }

        public List<string> Players { get; set; }

        public string MatchResultId { get; set; }

        public string TeamLeader { get; set; }

        public bool IsVerified { get; set; }
    }
}