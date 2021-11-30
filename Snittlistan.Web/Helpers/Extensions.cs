using System.Data.Entity;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Database;
using Snittlistan.Web.Infrastructure.Indexes;
using Snittlistan.Web.Models;

#nullable enable

namespace Snittlistan.Web.Helpers;
public static class Extensions
{
    public static async Task<Tenant> GetCurrentTenant(this Databases databases)
    {
        string hostname = CurrentHttpContext.Instance().Request.ServerVariables["SERVER_NAME"];
        Tenant loadedTenant = databases.Snittlistan.Tenants.Local.SingleOrDefault(x => x.Hostname == hostname);
        if (loadedTenant is not null)
        {
            return loadedTenant;
        }

        Tenant tenant = await databases.Snittlistan.Tenants.SingleOrDefaultAsync(x => x.Hostname == hostname);
        if (tenant == null)
        {
            throw new Exception($"No tenant found for hostname '{hostname}'");
        }

        return tenant;
    }

    public static User FindUserByEmail(this Raven.Client.IDocumentSession sess, string email)
    {
        return sess.Query<User, User_ByEmail>()
                   .FirstOrDefault(u => u.Email == email);
    }

    public static User FindUserByActivationKey(this Raven.Client.IDocumentSession sess, string key)
    {
        return sess.Query<User>()
                   .FirstOrDefault(u => u.ActivationKey == key);
    }

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
