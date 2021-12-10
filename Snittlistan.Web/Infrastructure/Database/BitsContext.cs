using Npgsql.NameTranslation;
using System.Data.Entity;

#nullable enable

namespace Snittlistan.Web.Infrastructure.Database;

public class BitsContext : DbContext, IBitsContext
{
    public IDbSet<Bits_Team> Teams { get; set; } = null!;

    public IDbSet<Bits_Hall> Hallar { get; set; } = null!;

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        System.Data.Entity.Database.SetInitializer(new NullDatabaseInitializer<BitsContext>());
        NpgsqlSnakeCaseNameTranslator mapper = new();
        _ = modelBuilder.HasDefaultSchema("bits");
        modelBuilder.Properties().Configure(x => x.HasColumnName(mapper.TranslateMemberName(x.ClrPropertyInfo.Name)));
        modelBuilder.Types().Configure(x => x.ToTable(mapper.TranslateMemberName(x.ClrType.Name.Replace("Bits", string.Empty))));
    }
}
