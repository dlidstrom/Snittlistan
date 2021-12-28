using Npgsql.NameTranslation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

#nullable enable

namespace Snittlistan.Web.Infrastructure.Database;

public class SnittlistanContext : DbContext, ISnittlistanContext
{
    public IDbSet<DelayedTask> DelayedTasks { get; set; } = null!;

    public IDbSet<PublishedTask> PublishedTasks { get; set; } = null!;

    public IDbSet<Tenant> Tenants { get; set; } = null!;

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        System.Data.Entity.Database.SetInitializer(new NullDatabaseInitializer<SnittlistanContext>());
        NpgsqlSnakeCaseNameTranslator mapper = new();
        _ = modelBuilder.HasDefaultSchema("snittlistan");
        modelBuilder.Properties().Configure(x => x.HasColumnName(mapper.TranslateMemberName(x.ClrPropertyInfo.Name)));
        modelBuilder.Types().Configure(x => x.ToTable(mapper.TranslateMemberName(x.ClrType.Name)));

        _ = modelBuilder.Entity<DelayedTask>()
            .Property(x => x.Version)
            .HasColumnName("xmin")
            .HasColumnType("text")
            .IsConcurrencyToken()
            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

        _ = modelBuilder.Entity<PublishedTask>()
            .Property(x => x.Version)
            .HasColumnName("xmin")
            .HasColumnType("text")
            .IsConcurrencyToken()
            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

        _ = modelBuilder.Entity<Tenant>()
            .Property(x => x.Version)
            .HasColumnName("xmin")
            .HasColumnType("text")
            .IsConcurrencyToken()
            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
    }
}
