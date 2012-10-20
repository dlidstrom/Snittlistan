namespace Snittlistan.Web.Infrastructure.Indexes
{
    using System.Linq;

    using Raven.Abstractions.Indexing;
    using Raven.Client.Indexes;

    using Snittlistan.Web.Models;

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
                                  };
            this.Store(x => x.Team, FieldStorage.Yes);
        }

        public class Result
        {
            public string Team { get; set; }

            public string Opponent { get; set; }

            public string Location { get; set; }
        }
    }
}