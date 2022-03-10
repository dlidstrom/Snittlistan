#nullable enable

using Npgsql.NameTranslation;

namespace Snittlistan.Web.Infrastructure.Database;

public class BitsContext : DbContext, IBitsContext
{
    public IDbSet<Bits_Team> Teams { get; set; } = null!;

    public IDbSet<Bits_Hall> Hallar { get; set; } = null!;

    public IDbSet<Bits_Match> Match { get; set; } = null!;

    public IDbSet<Bits_TeamRef> TeamRef { get; set; } = null!;

    public IDbSet<Bits_OilProfile> OilProfile { get; set; } = null!;

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        System.Data.Entity.Database.SetInitializer(new NullDatabaseInitializer<BitsContext>());
        NpgsqlSnakeCaseNameTranslator mapper = new();
        _ = modelBuilder.HasDefaultSchema("bits");
        modelBuilder.Properties().Configure(x => x.HasColumnName(
            mapper.TranslateMemberName(x.ClrPropertyInfo.Name)));
        modelBuilder.Types().Configure(x => x.ToTable(
            mapper.TranslateMemberName(x.ClrType.Name.Replace("Bits_", string.Empty))));

        _ = modelBuilder.Entity<Bits_Match>()
                    .HasRequired(p => p.AwayTeamRef)
                    .WithMany(x => x.Matches);
    }
}
