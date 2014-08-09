using System;

namespace Snittlistan.Web.Areas.V2.Domain
{
    public class ParseHeaderResult
    {
        public ParseHeaderResult(string team, string opponent, DateTime date, string location)
        {
            if (team == null) throw new ArgumentNullException("team");
            if (opponent == null) throw new ArgumentNullException("opponent");
            if (location == null) throw new ArgumentNullException("location");
            Team = team;
            Opponent = opponent;
            Date = date;
            Location = location;
        }

        public string Team { get; private set; }

        public string Opponent { get; private set; }

        public DateTime Date { get; private set; }

        public string Location { get; private set; }
    }
}