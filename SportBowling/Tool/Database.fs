module Database

open System
open System.Linq
open System.Text.RegularExpressions
open Microsoft.EntityFrameworkCore

type [<CLIMutable>] Division = {
    DivisionId : int
    ExternalDivisionId : string
    DivisionName : string
}

type DatabaseConnection = {
    Host : string
    Database : string
    Username : string
    Password : string
}
with
    override this.ToString() =
        $"Host=%s{this.Host};Database=%s{this.Database};Username=%s{this.Username};Password=%s{this.Password}"

type Context(databaseConnection : DatabaseConnection) =
    inherit DbContext()

    [<DefaultValue>]
    val mutable division : DbSet<Division>
    member x.Division
        with get() = x.division
        and set value = x.division <- value

    override _.OnConfiguring optionsBuilder =
        optionsBuilder.UseNpgsql(
            connectionString = databaseConnection.ToString(),
            //npgsqlOptionsAction = fun (x : Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.NpgsqlDbContextOptionsBuilder) -> ()) |> ignore
            npgsqlOptionsAction = null) |> ignore<DbContextOptionsBuilder>

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //         => optionsBuilder.UseNpgsql("Host=my_host;Database=my_db;Username=my_user;Password=my_pw");

    override _.OnModelCreating modelBuilder =
        let snakeCase s =
            if String.IsNullOrEmpty(s) then s
            else
                let r = Regex(@"[A-Z]{2,}(?=[A-Z][a-z]+[0-9]*|\b)|[A-Z]?[a-z]+[0-9]*|[A-Z]|[0-9]+")
                String.Join("_", r.Matches(s)).ToLower()
        let replaceColumnNames (entity : Metadata.IMutableEntityType) (property : Metadata.IMutableProperty) =
            let x = Metadata.StoreObjectIdentifier.Table(entity.GetTableName(), entity.GetSchema())
            property.SetColumnName(snakeCase(property.GetColumnName(&x)))
        let replaceTableNames (entity : Metadata.IMutableEntityType) =
            entity.SetTableName(snakeCase(entity.GetTableName()))
            entity.GetProperties()
            |> Seq.iter (fun t -> replaceColumnNames entity t)
        modelBuilder.Model.GetEntityTypes()
        |> Seq.iter replaceTableNames
    //     let esconvert = ValueConverter<EpisodeStatus, string>((fun v -> v.ToString()), (fun v -> Enum.Parse(typedefof<EpisodeStatus>, v) :?> EpisodeStatus))
    //     modelBuilder.Entity<Episode>().Property(fun e -> e.Status).HasConversion(esconvert) |> ignore
    //     let ssconvert = ValueConverter<SerieStatus, string>((fun v -> v.ToString()), (fun v -> Enum.Parse(typedefof<SerieStatus>, v) :?> SerieStatus))
    //     modelBuilder.Entity<Serie>().Property(fun e -> e.Status).HasConversion(ssconvert) |> ignore

type ContextFactory = unit -> Context

type Gateway(contextFactory : ContextFactory) =
    member _.StoreDivision(bitsDivisions : Contracts.Bits.Divisions.Division array) =
        use context = contextFactory()

        let divisions = context.Division.ToList()
        printfn "Number of divisions: %d" divisions.Count
        // context.Division
        // read divisions
        // division.data <- 1
