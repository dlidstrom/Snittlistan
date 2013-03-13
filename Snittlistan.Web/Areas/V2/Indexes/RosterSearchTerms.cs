using System;
using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Web.Areas.V2.Indexes
{
    public class RosterSearchTerms : AbstractIndexCreationTask<Roster, RosterSearchTerms.Result>
    {
        public RosterSearchTerms()
        {
            this.Map = rosters => from roster in rosters
                                  select new
                                  {
                                      roster.Id,
                                      roster.Team,
                                      roster.Opponent,
                                      roster.Location,
                                      roster.Turn,
                                      roster.Season,
                                      roster.Date,
                                      roster.MatchResultId
                                  };

            Store(x => x.Id, FieldStorage.Yes);
        }

        public class Result
        {
            public string Id { get; set; }
            public string Team { get; set; }
            public string Opponent { get; set; }
            public string Location { get; set; }
            public int Turn { get; set; }
            public int Season { get; set; }
            public DateTime Date { get; set; }
            public string MatchResultId { get; set; }
        }
    }
}