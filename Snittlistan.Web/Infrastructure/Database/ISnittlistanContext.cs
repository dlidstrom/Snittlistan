#nullable enable

using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Snittlistan.Web.Infrastructure.Database;

public interface ISnittlistanContext
{
    IDbSet<DelayedTask> DelayedTasks { get; }

    IDbSet<PublishedTask> PublishedTasks { get; }

    IDbSet<Tenant> Tenants { get; }

    IDbSet<RosterMail> RosterMails { get; }

    DbChangeTracker ChangeTracker { get; }

    Task<int> SaveChangesAsync();

    int SaveChanges();
}
