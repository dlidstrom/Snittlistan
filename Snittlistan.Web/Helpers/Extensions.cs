#nullable enable

using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Indexes;

namespace Snittlistan.Web.Helpers;

public static class Extensions
{
    public static int LatestSeasonOrDefault(this Raven.Client.IDocumentSession sess, int def)
    {
        return sess.Query<Roster, RosterSearchTerms>()
                   .OrderByDescending(s => s.Season)
                   .Select(r => r.Season)
                   .ToList()
                   .DefaultIfEmpty(def)
                   .First();
    }
}
