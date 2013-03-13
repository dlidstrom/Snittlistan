using System;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class TurnResultViewModel
    {
        public TurnResultViewModel()
        {
        }

        public int Id { get; set; }

        public string Date { get; private set; }

        public string Location { get; private set; }

        public string Team { get; private set; }

        public string Opponent { get; private set; }

        public int TeamScore { get; private set; }

        public int OpponentScore { get; private set; }

        public int BitsMatchId { get; private set; }

        public char TeamLevel
        {
            get
            {
                if (this.Team.Length < 1) throw new InvalidOperationException("Initialize Team first");
                return char.ToLower(this.Team[this.Team.Length - 1]);
            }
        }
    }
}