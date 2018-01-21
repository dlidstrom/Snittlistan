using System;
using System.Collections.Generic;
using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class RosterViewModel
    {
        public RosterViewModel()
        {
            Players = new List<Tuple<string, string>>();
        }

        public RosterViewModel(Roster roster, Tuple<string, string> teamLeader, List<Tuple<string, string>> players)
        {
            TeamLeader = teamLeader;
            Players = players;
            Id = roster.Id;
            Season = roster.Season;
            Turn = roster.Turn;
            Team = roster.Team;
            TeamLevel = roster.TeamLevel;
            Location = roster.Location;
            Opponent = roster.Opponent;
            OilPattern = roster.OilPattern;
            Date = roster.Date;
            IsFourPlayer = roster.IsFourPlayer;
            Time = roster.Date.ToShortTimeString();
            Preliminary = roster.Preliminary;
        }

        public string Id { get; set; }

        public int Season { get; set; }

        public int Turn { get; set; }

        public string Team { get; set; }

        public string TeamLevel { get; set; }

        public string Location { get; set; }

        public string Opponent { get; set; }

        public OilPatternInformation OilPattern { get; set; }

        public DateTime Date { get; set; }

        public bool IsFourPlayer { get; set; }

        public string Time { get; set; }

        public bool Preliminary { get; set; }

        public List<Tuple<string, string>> Players { get; set; }

        public Tuple<string, string> TeamLeader { get; set; }
    }
}