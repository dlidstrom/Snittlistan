#nullable enable

using System.Data.Entity.Infrastructure;

namespace Snittlistan.Web.Infrastructure.Database;

public interface ISnittlistanContext
{
    IDbSet<PublishedTask> PublishedTasks { get; }

    IDbSet<Tenant> Tenants { get; }

    IDbSet<RosterMail> RosterMails { get; }

    IDbSet<ChangeLog> ChangeLogs { get; }

    IDbSet<KeyValueProperty> KeyValueProperties { get; }

    IDbSet<SentEmail> SentEmails { get; }

    DbChangeTracker ChangeTracker { get; }

    Task<int> SaveChangesAsync();

    int SaveChanges();
}
