module Entities

open Microsoft.EntityFrameworkCore

type [<CLIMutable>] Cache = {
    Id : int
}

type Context(connectionString) =
    inherit DbContext()

    [<DefaultValue>]
    val mutable cache:DbSet<Cache>
    member x.Cache
        with get() = x.cache
        and set value = x.cache <- value

    override _.OnConfiguring optionsBuilder =
        optionsBuilder.UseNpgsql(connectionString, fun (x : NpgsqlDbContextOptionsBuilder) -> ()) |> ignore

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //         => optionsBuilder.UseNpgsql("Host=my_host;Database=my_db;Username=my_user;Password=my_pw");

    // override __.OnModelCreating modelBuilder =
    //     let esconvert = ValueConverter<EpisodeStatus, string>((fun v -> v.ToString()), (fun v -> Enum.Parse(typedefof<EpisodeStatus>, v) :?> EpisodeStatus))
    //     modelBuilder.Entity<Episode>().Property(fun e -> e.Status).HasConversion(esconvert) |> ignore
    //     let ssconvert = ValueConverter<SerieStatus, string>((fun v -> v.ToString()), (fun v -> Enum.Parse(typedefof<SerieStatus>, v) :?> SerieStatus))
    //     modelBuilder.Entity<Serie>().Property(fun e -> e.Status).HasConversion(ssconvert) |> ignore
