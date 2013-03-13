using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Web.Helpers
{
    using System;
    using System.Linq;

    using Raven.Client;
    using Raven.Client.Linq;

    using Snittlistan.Web.Areas.V2.Indexes;
    using Snittlistan.Web.Infrastructure.Indexes;
    using Snittlistan.Web.Models;

    public static class DocumentSessionExtensions
    {
        public static User FindUserByEmail(this IDocumentSession sess, string email)
        {
            return sess.Query<User, User_ByEmail>()
                .FirstOrDefault(u => u.Email == email);
        }

        public static User FindUserByActivationKey(this IDocumentSession sess, string key)
        {
            return sess.Query<User>()
                .FirstOrDefault(u => u.ActivationKey == key);
        }

        public static bool BitsIdExists(this IDocumentSession sess, int id)
        {
            return sess.Query<Match_ByBitsMatchId.Result, Match_ByBitsMatchId>()
                .AsProjection<Match_ByBitsMatchId.Result>()
                .SingleOrDefault(m => m.BitsMatchId == id) != null;
        }

        public static int LatestSeasonOrDefault(this IDocumentSession sess, int def)
        {
            return sess.Query<Roster, RosterSearchTerms>()
                .OrderByDescending(s => s.Season)
                .Select(r => r.Season)
                .ToList()
                .DefaultIfEmpty(def)
                .First();
        }

    }
}