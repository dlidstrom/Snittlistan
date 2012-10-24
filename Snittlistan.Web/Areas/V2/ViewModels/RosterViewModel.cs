namespace Snittlistan.Web.Areas.V2.ViewModels
{
    using System;
    using System.Collections.Generic;

    public class RosterViewModel
    {
        public RosterViewModel()
        {
            Players = new List<Tuple<string, string>>();
        }

        public int Id { get; set; }

        public int Season { get; set; }

        public int Turn { get; set; }

        public string Team { get; set; }

        public string Location { get; set; }

        public string Opponent { get; set; }

        public DateTime Date { get; set; }

        public string Time { get; set; }

        public char TeamLevel
        {
            get
            {
                if (this.Team.Length < 1) throw new InvalidOperationException("Initialize Team first");
                return char.ToLower(this.Team[this.Team.Length - 1]);
            }
        }

        public List<Tuple<string, string>> Players { get; set; }
    }
}