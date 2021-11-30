using Npgsql.NameTranslation;

#nullable enable

namespace Snittlistan.Web.Infrastructure.Database;

public class SnittlistanContext : System.Data.Entity.DbContext, ISnittlistanContext
{
    public System.Data.Entity.IDbSet<DelayedTask> DelayedTasks { get; set; } = null!;

    public System.Data.Entity.IDbSet<PublishedTask> PublishedTasks { get; set; } = null!;

    public System.Data.Entity.IDbSet<Tenant> Tenants { get; set; } = null!;

    protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        System.Data.Entity.Database.SetInitializer(new System.Data.Entity.NullDatabaseInitializer<SnittlistanContext>());
        NpgsqlSnakeCaseNameTranslator mapper = new();
        _ = modelBuilder.HasDefaultSchema("snittlistan");
        modelBuilder.Properties().Configure(x => x.HasColumnName(mapper.TranslateMemberName(x.ClrPropertyInfo.Name)));
        modelBuilder.Types().Configure(x => x.ToTable(mapper.TranslateMemberName(x.ClrType.Name)));
    }
}
