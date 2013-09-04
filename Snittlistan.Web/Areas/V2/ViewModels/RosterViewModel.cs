using System;
using System.Collections.Generic;
using System.Linq;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class RosterViewModel
    {
        public RosterViewModel()
        {
            Players = new List<Tuple<string, string>>();
        }

        public string Id { get; set; }

        public int Season { get; set; }

        public int Turn { get; set; }

        public string Team { get; set; }

        public string Location { get; set; }

        public string Opponent { get; set; }

        public DateTime Date { get; set; }

        public bool IsFourPlayer { get; set; }

        public string Time { get; set; }

        public bool Preliminary { get; set; }

        public char TeamLevel
        {
            get
            {
                if (Team.Length < 1) throw new InvalidOperationException("Initialize Team first");
                return char.ToLower(Team.Last());
            }
        }

        public List<Tuple<string, string>> Players { get; set; }
    }
}