using System;

namespace Snittlistan.Web.Areas.V2.Domain
{
    public class ParseMatchSchemeResult
    {
        public ParseMatchSchemeResult(MatchItem[] matches)
        {
            Matches = matches;
        }

        public MatchItem[] Matches { get; }

        public class MatchItem
        {
            public int Turn { get; set; }
            public DateTime Date { get; set; }
            public int BitsMatchId { get; set; }
            public string Teams { get; set; }
            public string MatchResult { get; set; }
            public string OilPatternName { get; set; }
            public int OilPatternId { get; set; }
            public string Location { get; set; }
            public string LocationUrl { get; set; }
        }
    }
}