#nullable enable

namespace Snittlistan.Web.Infrastructure.Database
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Threading.Tasks;

    public interface ISnittlistanContext
    {
        IDbSet<DelayedTask> DelayedTasks { get; }

        IDbSet<PublishedTask> PublishedTasks { get; }

        IDbSet<Tenant> Tenants { get; }

        DbChangeTracker ChangeTracker { get; }

        Task<int> SaveChangesAsync();

        int SaveChanges();
    }
}
