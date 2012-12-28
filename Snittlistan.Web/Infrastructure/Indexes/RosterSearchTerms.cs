namespace Snittlistan.Web.Infrastructure.Indexes
{
    using System.Linq;

    using Raven.Client.Indexes;

    using Snittlistan.Web.Areas.V2.Models;

    public class RosterSearchTerms : AbstractIndexCreationTask<Roster, RosterSearchTerms.Result>
    {
        public RosterSearchTerms()
        {
            this.Map = rosters => from roster in rosters
                                  select new
                                  {
                                      roster.Team,
                                      roster.Opponent,
                                      roster.Location,
                                      roster.Turn,
                                      roster.Season,
                                      roster.Date
                                  };
        }

        public class Result
        {
            public string Team { get; set; }

            public string Opponent { get; set; }

            public string Location { get; set; }

            public int Turn { get; set; }

            public int Season { get; set; }
        }
    }
}