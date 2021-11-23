#nullable enable

namespace Snittlistan.Web.Infrastructure.Database
{
    using System.Data.Entity;
    using Npgsql.NameTranslation;

    public class BitsContext : DbContext, IBitsContext
    {
        public IDbSet<Bits_Team> Teams { get; set; } = null!;

        public IDbSet<Bits_Hall> Hallar { get; set; } = null!;

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Database.SetInitializer(new NullDatabaseInitializer<BitsContext>());
            NpgsqlSnakeCaseNameTranslator mapper = new();
            _ = modelBuilder.HasDefaultSchema("bits");
            modelBuilder.Properties().Configure(x => x.HasColumnName(mapper.TranslateMemberName(x.ClrPropertyInfo.Name)));
            modelBuilder.Types().Configure(x => x.ToTable(mapper.TranslateMemberName(x.ClrType.Name.Replace("Bits", string.Empty))));
        }
    }
}
