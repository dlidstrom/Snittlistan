module Database

open System
open System.Linq
open Microsoft.EntityFrameworkCore
open Microsoft.EntityFrameworkCore.Storage.ValueConversion
open Npgsql
open Npgsql.NameTranslation
open EntityFrameworkCore.FSharp

module Entities =
    type [<CLIMutable>] Division = {
        DivisionId : int
        ExternalDivisionId : string
        DivisionName : string
    }

    type HttpMethod = Get | Post
    type [<CLIMutable>] Request = {
        RequestId : int
        Url : string
        Method : HttpMethod
        Body : string option
    }

type DatabaseConnection = {
    Host : string
    Database : string
    Username : string
    Password : string
}
with
    member this.Format() =
        $"Host=%s{this.Host};Database=%s{this.Database};Username=%s{this.Username};Password=%s{this.Password}"

type Context(databaseConnection : DatabaseConnection, loggerFactory) =
    inherit DbContext()

    [<DefaultValue>]
    val mutable division : DbSet<Entities.Division>
    member x.Division
        with get() = x.division
        and set value = x.division <- value

    [<DefaultValue>]
    val mutable requests : DbSet<Entities.Request>
    member x.Requests
        with get() = x.requests
        and set value = x.requests <- value

    override _.OnConfiguring optionsBuilder =
        optionsBuilder
            .UseNpgsql(connectionString = databaseConnection.ToString())
            .UseLoggerFactory(loggerFactory)
            |> ignore<DbContextOptionsBuilder>

    override _.OnModelCreating modelBuilder =
        let snakeCase s =
            NpgsqlSnakeCaseNameTranslator.ConvertToSnakeCase s
        let replaceColumnNames (entity : Metadata.IMutableEntityType) (property : Metadata.IMutableProperty) =
            let x = Metadata.StoreObjectIdentifier.Table(entity.GetTableName(), entity.GetSchema())
            property.SetColumnName(snakeCase(property.GetColumnName(&x)))
        let replaceTableNames (entity : Metadata.IMutableEntityType) =
            entity.SetTableName(snakeCase(entity.GetTableName()))
            entity.GetProperties()
            |> Seq.iter (fun t -> replaceColumnNames entity t)
        modelBuilder.Model.GetEntityTypes()
        |> Seq.iter replaceTableNames
        let httpMethodConvert = ValueConverter<Entities.HttpMethod, string>((fun v -> v.ToString()), (fun v -> Enum.Parse(typedefof<Entities.HttpMethod>, v) :?> Entities.HttpMethod))
        modelBuilder.Entity<Entities.Request>().Property(fun e -> e.Method).HasConversion(httpMethodConvert) |> ignore
        modelBuilder.Entity<Entities.Request>().Property(fun e -> e.Body).HasConversion(OptionConverter()) |> ignore

type ContextFactory = unit -> Context

type Gateway(
            contextFactory : ContextFactory,
            connection : NpgsqlConnection) =

    let run f =
        use transaction = connection.BeginTransaction()
        let result =
            connection
            |> FSharp.Sql.existingConnection
            |> f
        transaction.Commit()
        result

    member _.StoreDivision
        (bitsDivisions : Contracts.Bits.Divisions.Division array) =

        // how to do batch?
        run
        <| FSharp.Sql.query
            "INSERT INTO bits.division \
                (external_division_id, division_name)"
        |> FSharp.Sql.parameters
            [
                "@external", Sql.int 1
            ]
        |> FSharp.Sql.executeNonQuery
        |> function
        | Ok items -> printf "Stored %d items" items
        | Error err -> raise err
        use context = contextFactory()

        let divisions = context.Division.ToList()
        printfn "Number of divisions: %d" divisions.Count
        // context.Division
        // read divisions
        // division.data <- 1

    member _.StoreRequest
        url
        (method : Entities.HttpMethod)
        body =
        run
        <| FSharp.Sql.query
            "INSERT INTO bits.request \
                (url, method, body, created_utc) \
             VALUES (@url, @method, @body, @created_utc)"
        |> FSharp.Sql.parameters
            [
                "@url", Sql.string url
                "@method", Sql.string (method.ToString())
                "@body", Sql.string body
                "@created_utc", Sql.timestamp DateTime.UtcNow
            ]
        |> FSharp.Sql.executeNonQuery
        |> function
        | Ok items -> printf "Stored %d items" items
        | Error err -> raise err
