using Npgsql.NameTranslation;

#nullable enable

namespace Snittlistan.Web.Infrastructure.Database;

public class BitsContext : System.Data.Entity.DbContext, IBitsContext
{
    public System.Data.Entity.IDbSet<Bits_Team> Teams { get; set; } = null!;

    public System.Data.Entity.IDbSet<Bits_Hall> Hallar { get; set; } = null!;

    protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        System.Data.Entity.Database.SetInitializer(new System.Data.Entity.NullDatabaseInitializer<BitsContext>());
        NpgsqlSnakeCaseNameTranslator mapper = new();
        _ = modelBuilder.HasDefaultSchema("bits");
        modelBuilder.Properties().Configure(x => x.HasColumnName(mapper.TranslateMemberName(x.ClrPropertyInfo.Name)));
        modelBuilder.Types().Configure(x => x.ToTable(mapper.TranslateMemberName(x.ClrType.Name.Replace("Bits", string.Empty))));
    }
}
