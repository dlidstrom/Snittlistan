using System;
using System.Linq;
using Raven.Client;
using Raven.Client.Linq;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Web.Infrastructure;

namespace Snittlistan.Web.Areas.V2.Queries
{
    public class GetPendingMatchesQuery : IQuery<Roster[]>
    {
        public Roster[] Execute(IDocumentSession session)
        {
            var results = session.Query<RosterSearchTerms.Result, RosterSearchTerms>()
                                 .Where(x => x.Preliminary == false)
                                 .Where(x => x.Date < DateTime.Now)
                                 .Where(x => x.BitsMatchId != 0)
                                 .Where(x => x.MatchResultId == null)
                                 .OrderBy(x => x.Date)
                                 .ProjectFromIndexFieldsInto<RosterSearchTerms.Result>()
                                 .ToArray();

            var rosters = session.Include<Roster>(x => x.Players).Load<Roster>(results.Select(x => x.Id));
            return rosters;
        }
    }
}