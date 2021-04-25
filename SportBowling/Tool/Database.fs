module Database

open System
open FSharp.Data.Sql
open FSharp.Data.Sql.Common
open Npgsql
open Npgsql.FSharp

module Entities =
    type Division =
        { DivisionId: int
          ExternalDivisionId: string
          DivisionName: string }

    type HttpMethod =
        | Get
        | Post

    type Request =
        { RequestId: int
          Url: string
          Method: HttpMethod
          Body: string option }

type DatabaseConnection =
    { Host: string
      Database: string
      Username: string
      Password: string }
    member this.Format() =
        stringBuffer {
            $"Host=%s{this.Host};"
            $"Database=%s{this.Database};"
            $"Username=%s{this.Username};"
            $"Password=%s{this.Password}"
        }

[<Literal>]
let DbVendor = DatabaseProviderTypes.POSTGRESQL

[<Literal>]
let ConnString =
    "Host=localhost;Port=5432;Database=prisma;Username=prisma;Password=prisma"

[<Literal>]
let ResPath = __SOURCE_DIRECTORY__ + @"./lib"

[<Literal>]
let IndivAmount = 1000

type LogProvider =
    SqlDataProvider<DatabaseVendor=DbVendor, ConnectionString=ConnString, ResolutionPath=ResPath, IndividualsAmount=IndivAmount, UseOptionTypes=true, Owner="log">

type BitsProvider =
    SqlDataProvider<DatabaseVendor=DbVendor, ConnectionString=ConnString, ResolutionPath=ResPath, IndividualsAmount=IndivAmount, UseOptionTypes=true, Owner="bits">

type Gateway(connection: NpgsqlConnection, databaseConnection: DatabaseConnection) =

    member _.StoreDivision2
        (seasonId: Domain.SeasonId)
        (bitsDivisions: Contracts.Bits.Divisions.Division array)
        =
        let (Domain.SeasonId seasonIdUnwrapped) = seasonId
        let ctx = BitsProvider.GetDataContext()

        bitsDivisions
        |> Array.map
            (fun d ->
                ctx.Bits.Division.Create(
                    DateTime.UtcNow,
                    d.DivisionName,
                    d.DivisionId,
                    seasonIdUnwrapped
                ))
        |> ignore

        ctx.SubmitUpdates()

    member _.StoreDivision(bitsDivisions: Contracts.Bits.Divisions.Division array) =

        let insertDivision (division: Contracts.Bits.Divisions.Division) =
            connection
            |> Sql.existingConnection
            |> Sql.query
                "INSERT INTO bits.division \
                 (external_division_id, division_name, created_utc) \
                 VALUES (@external_division_id, @division_name, @created_utc)"
            |> Sql.parameters [ "@external_division_id", Sql.int division.DivisionId
                                "@division_name", Sql.string division.DivisionName
                                "@created_utc", Sql.timestamp DateTime.UtcNow ]
            |> Sql.executeNonQuery
            |> printfn "Stored %d items"

        use transaction = connection.BeginTransaction()
        bitsDivisions |> Array.iter insertDivision
        transaction.Commit()

    member _.StoreRequest url (method: Entities.HttpMethod) body =
        let insertRequest () =
            connection
            |> Sql.existingConnection
            |> Sql.query
                "INSERT INTO log.request \
                 (url, method, body, created_utc) \
                 VALUES (@url, @method, @body, @created_utc)
                 RETURNING request_id"
            |> Sql.parameters [ "@url", Sql.string url
                                "@method", Sql.string (method.ToString())
                                "@body", Sql.string body
                                "@created_utc", Sql.timestamp DateTime.UtcNow ]
            |> Sql.executeRow (fun read -> read.int "request_id")

        use transaction = connection.BeginTransaction()
        let requestId = insertRequest ()
        transaction.Commit()
        requestId

    member _.StoreResponse requestId statusCode body contentLength contentType =
        let insertResponse () =
            connection
            |> Sql.existingConnection
            |> Sql.query
                "INSERT INTO log.response \
                (request_id, status_code, body, content_length, content_type) \
                VALUES (@request_id, @status_code, @body, @content_length, @content_type) \
                RETURNING response_id"
            |> Sql.parameters [ "@request_id", Sql.int requestId
                                "@status_code", Sql.int statusCode
                                "@body", Sql.string body
                                "@content_length", Sql.int contentLength
                                "@content_type", Sql.string contentType ]
            |> Sql.executeRow (fun read -> read.int "response_id")

        use transaction = connection.BeginTransaction()
        let responseId = insertResponse ()
        transaction.Commit()
        responseId

    member _.GetRequest url (method: Entities.HttpMethod) body =
        let getRequest () =
            connection
            |> Sql.existingConnection
            |> Sql.query "SELECT body FROM log.response WHERE "

        use transaction = connection.BeginTransaction()
        transaction.Commit()
