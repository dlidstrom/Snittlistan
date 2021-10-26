#nullable enable

namespace Snittlistan.Queue.Infrastructure
{
    using System.Data.Entity;
    using Npgsql.NameTranslation;

    public class DatabaseContext : DbContext
    {
        public DbSet<Tenant> Tenants { get; set; } = null!;

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Database.SetInitializer(new NullDatabaseInitializer<DatabaseContext>());
            NpgsqlSnakeCaseNameTranslator mapper = new();
            _ = modelBuilder.HasDefaultSchema("public");
            modelBuilder.Properties().Configure(x => x.HasColumnName(mapper.TranslateMemberName(x.ClrPropertyInfo.Name)));
            modelBuilder.Types().Configure(x => x.ToTable(mapper.TranslateMemberName(x.ClrType.Name)));
        }
    }
}
