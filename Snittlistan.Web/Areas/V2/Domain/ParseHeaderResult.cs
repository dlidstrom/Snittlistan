#nullable enable

namespace Snittlistan.Web.Areas.V2.Domain
{
    using System;

    public class ParseHeaderResult
    {
        public ParseHeaderResult(
            string team,
            string opponent,
            int turn,
            DateTime date,
            string location,
            OilPatternInformation oilPattern)
        {
            Team = team ?? throw new ArgumentNullException(nameof(team));
            Opponent = opponent ?? throw new ArgumentNullException(nameof(opponent));
            Turn = turn;
            Date = date;
            Location = location ?? throw new ArgumentNullException(nameof(location));
            OilPattern = oilPattern;
        }

        public string Team { get; }

        public string Opponent { get; }

        public int Turn { get; }

        public DateTime Date { get; }

        public string Location { get; }

        public OilPatternInformation OilPattern { get; }
    }
}
