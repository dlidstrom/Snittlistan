using System;

namespace Snittlistan.Web.Areas.V2.Domain
{
    public class ParseHeaderResult
    {
        public ParseHeaderResult(string team, string opponent, DateTime date, string location)
        {
            Team = team ?? throw new ArgumentNullException(nameof(team));
            Opponent = opponent ?? throw new ArgumentNullException(nameof(opponent));
            Date = date;
            Location = location ?? throw new ArgumentNullException(nameof(location));
        }

        public string Team { get; private set; }

        public string Opponent { get; private set; }

        public DateTime Date { get; private set; }

        public string Location { get; private set; }
    }
}