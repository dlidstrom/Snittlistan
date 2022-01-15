#nullable enable

using Npgsql.NameTranslation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Snittlistan.Web.Infrastructure.Database;

public class SnittlistanContext : DbContext, ISnittlistanContext
{
    public IDbSet<PublishedTask> PublishedTasks { get; set; } = null!;

    public IDbSet<Tenant> Tenants { get; set; } = null!;

    public IDbSet<RosterMail> RosterMails { get; set; } = null!;

    public IDbSet<ChangeLog> ChangeLogs { get; set; } = null!;

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        System.Data.Entity.Database.SetInitializer(new NullDatabaseInitializer<SnittlistanContext>());
        NpgsqlSnakeCaseNameTranslator mapper = new();
        _ = modelBuilder.HasDefaultSchema("snittlistan");
        modelBuilder.Properties().Configure(x => x.HasColumnName(mapper.TranslateMemberName(x.ClrPropertyInfo.Name)));
        modelBuilder.Types().Configure(x => x.ToTable(mapper.TranslateMemberName(x.ClrType.Name)));

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

        _ = modelBuilder.Entity<RosterMail>()
            .Property(x => x.Version)
            .HasColumnName("xmin")
            .HasColumnType("text")
            .IsConcurrencyToken()
            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
    }
}
