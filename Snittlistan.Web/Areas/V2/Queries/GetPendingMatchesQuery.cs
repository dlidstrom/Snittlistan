namespace Snittlistan.Web.Areas.V2.Queries
{
    using System;
    using System.Linq;
    using Raven.Client;
    using Raven.Client.Linq;
    using Snittlistan.Web.Areas.V2.Domain;
    using Snittlistan.Web.Areas.V2.Indexes;
    using Snittlistan.Web.Infrastructure;

    public class GetPendingMatchesQuery : IQuery<Roster[]>
    {
        private readonly int seasonId;

        public GetPendingMatchesQuery(int seasonId)
        {
            this.seasonId = seasonId;
        }

        public Roster[] Execute(IDocumentSession session)
        {
            RosterSearchTerms.Result[] results = session.Query<RosterSearchTerms.Result, RosterSearchTerms>()
                                 .Where(x => x.Preliminary == false)
                                 .Where(x => x.Date < DateTime.Now)
                                 .Where(x => x.BitsMatchId != 0)
                                 .Where(x => x.MatchResultId == null)
                                 .Where(x => x.Season == seasonId)
                                 .OrderBy(x => x.Date)
                                 .ProjectFromIndexFieldsInto<RosterSearchTerms.Result>()
                                 .ToArray();

            Roster[] rosters = session.Include<Roster>(x => x.Players).Load<Roster>(results.Select(x => x.Id));
            return rosters;
        }
    }
}