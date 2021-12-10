
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

#nullable enable

namespace Snittlistan.Web.Infrastructure.Database;
public interface ISnittlistanContext
{
    IDbSet<DelayedTask> DelayedTasks { get; }

    IDbSet<PublishedTask> PublishedTasks { get; }

    IDbSet<Tenant> Tenants { get; }

    DbChangeTracker ChangeTracker { get; }

    Task<int> SaveChangesAsync();

    int SaveChanges();
}
