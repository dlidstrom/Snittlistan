#nullable enable

using Npgsql.NameTranslation;
using System.Data.Entity;

namespace Snittlistan.Web.Infrastructure.Database;

public class SnittlistanContext : DbContext, ISnittlistanContext
{
    public IDbSet<PublishedTask> PublishedTasks { get; set; } = null!;

    public IDbSet<Tenant> Tenants { get; set; } = null!;

    public IDbSet<RosterMail> RosterMails { get; set; } = null!;

    public IDbSet<ChangeLog> ChangeLogs { get; set; } = null!;

    public IDbSet<RateLimit> RateLimits { get; set; } = null!;

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        System.Data.Entity.Database.SetInitializer(new NullDatabaseInitializer<SnittlistanContext>());
        NpgsqlSnakeCaseNameTranslator mapper = new();
        _ = modelBuilder.HasDefaultSchema("snittlistan");
        modelBuilder.Properties().Configure(x => x.HasColumnName(mapper.TranslateMemberName(x.ClrPropertyInfo.Name)));
        modelBuilder.Types().Configure(x => x.ToTable(mapper.TranslateMemberName(x.ClrType.Name)));
    }
}
