namespace Snittlistan.Web.Infrastructure.Database
{
    using System.Data.Entity;
    using Npgsql.NameTranslation;

    public class DatabaseContext : DbContext
    {
        public DbSet<Query> Queries { get; set; }
        public DbSet<DelayedTask> DelayedTasks { get; set; }

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

    public class Query
    {
        public int QueryId { get; set; }
    }

    public class DelayedTask
    {
        public int DelayedTaskId { get; set; }
    }
}
