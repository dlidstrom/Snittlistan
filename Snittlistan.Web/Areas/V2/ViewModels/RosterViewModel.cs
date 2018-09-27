using Snittlistan.Web.Areas.V2.Domain;
using System;
using System.Collections.Generic;

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
            Season = roster.Season;
            Turn = roster.Turn;
            IsFourPlayer = roster.IsFourPlayer;
            Preliminary = roster.Preliminary;
            Header = new RosterHeaderViewModel(
                roster.Id,
                roster.Team,
                roster.TeamLevel,
                roster.Location,
                roster.Opponent,
                roster.Date,
                roster.OilPattern,
                roster.MatchResultId);
        }

        public RosterHeaderViewModel Header { get; set; }

        public int Season { get; set; }

        public int Turn { get; set; }

        public bool IsFourPlayer { get; set; }

        public bool Preliminary { get; set; }

        public List<Tuple<string, string>> Players { get; set; }

        public Tuple<string, string> TeamLeader { get; set; }
    }
}