// dotnet watch run --no-restore -- --api-key 123 --season-id 2006

open System
open Argu
open DbUp.Engine
open NLog
open Npgsql

open Infrastructure

type Arguments =
    | [<CliPrefix(CliPrefix.None)>] Fetch_Matches of ParseResults<FetchMatchesArguments>
    | [<Mandatory>] Host of host: string
    | [<Mandatory>] Database of database: string
    | [<Mandatory>] Username of username: string
    | [<Mandatory>] Password of password: string
    | Debug_Http
    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Fetch_Matches _ -> "Fetch matches."
            | Host _ -> "Specifies the database host."
            | Database _ -> "Specifies the database name."
            | Username _ -> "Specifies the username"
            | Password _ -> "Specifies the password."
            | Debug_Http -> "Debug HTTP."

and FetchMatchesArguments =
    | [<Mandatory>] Api_Key of apiKey: string
    | [<Mandatory>] Season_Id of seasonId: int
    | No_Check_Certificate
    | Proxy of proxy: string
    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Api_Key _ -> "Specifies the ApiKey."
            | Season_Id _ -> "specifies the season id."
            | No_Check_Certificate ->
                "Don't check the server certificate against \
                 the available certificate authorities."
            | Proxy _ -> "Specifies the proxy to use."

and ScratchArguments =
    | Verbose
    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Verbose -> "Verbose output."

let fetchMatches
    (args: ParseResults<FetchMatchesArguments>)
    (connection: Database.DatabaseConnection)
    (logger: Contracts.Logger)
    =
    let apiKey = args.GetResult Api_Key
    let seasonId = args.GetResult Season_Id

    let noCheckCertificate =
        if args.Contains(No_Check_Certificate) then
            Some true
        else
            None

    let proxy = args.TryGetResult(Proxy)

    use npgsqlConnection =
        new NpgsqlConnection(connection.Format())

    npgsqlConnection.Open()
    // let gateway = Database.Gateway(npgsqlConnection) // TODO REMOVE
    let http = BitsHttp.create proxy noCheckCertificate

    let logHttp (request: Contracts.RequestDefinition) =
        match request.Method with
        | Contracts.Method.Get -> logger.Log $"GET %s{request.Url}"
        | Contracts.Method.Post -> logger.Log $"POST %s{request.Url}"

        http request

    // cache impl
    let cachedHttp (request: Contracts.RequestDefinition) =
        let database = Database.Gateway(npgsqlConnection, connection)
        CachedApi.cachedHttp http database (TimeSpan.FromHours(1.0)) request

    let bitsClient = Api.Bits.Client(apiKey, cachedHttp)

    let workflow =
        Workflows.FetchMatchesWorkflow(bitsClient, logger)

    workflow.Run(Domain.SeasonId seasonId)

let run argv logger =
    let parser =
        ArgumentParser.Create<Arguments>(programName = AppDomain.CurrentDomain.FriendlyName)

    let results = parser.ParseCommandLine argv

    let host = results.GetResult Host
    let database = results.GetResult Database
    let username = results.GetResult Username
    let password = results.GetResult Password

    let databaseConnection : Database.DatabaseConnection =
        { Host = host
          Database = database
          Username = username
          Password = password }

    let debugHttp = results.Contains(Debug_Http)

    let _l =
        if debugHttp then
            Some(new LoggingEventListener())
        else
            None

    match results.GetSubCommand() with
    | Fetch_Matches args -> fetchMatches args databaseConnection logger
    | Debug_Http -> failwith "Unexpected"
    | Host _ -> failwith "Unexpected"
    | Database _ -> failwith "Unexpected"
    | Username _ -> failwith "Unexpected"
    | Password _ -> failwith "Unexpected"

module SqlTest =
    open FSharp.Data.Sql
    open FSharp.Data.Sql.Common

    [<Literal>]
    let DbVendor = Common.DatabaseProviderTypes.POSTGRESQL

    [<Literal>]
    let ConnString =
        "Host=localhost;Port=5432;Database=prisma;Username=prisma;Password=prisma"

    [<Literal>]
    let Schema = "public, log, bits"

    [<Literal>]
    let ResPath = __SOURCE_DIRECTORY__ + @"./lib"

    [<Literal>]
    let IndivAmount = 1000

    [<Literal>]
    let UseOptTypes = true

    type DB =
        SqlDataProvider<DatabaseVendor=DbVendor, ConnectionString=ConnString, ResolutionPath=ResPath, IndividualsAmount=IndivAmount, UseOptionTypes=UseOptTypes, Owner=Schema>

    let ctx =
        DB.GetDataContext(selectOperations = SelectOperations.DatabaseSide)

    QueryEvents.SqlQueryEvent
    |> Event.add (printfn "Executing query: %O")

    let queryBits s =
        query {
            for season in ctx.Bits.Season do
                where (season.StartYear = s)
                select (Some season)
                headOrDefault
                //exactlyOneOrDefault
        }

[<EntryPoint>]
let main argv =
    try
        let nlogger = LogManager.GetLogger("main")

        let logger =
            { new Contracts.Logger with
                member _.Log s = info nlogger s }

        logger.Log "starting"
        let season = SqlTest.queryBits 2020
        match season with
        | Some s -> printfn "%d" s.StartYear
        | None -> ()
        season |> printfn "%A"
        run argv logger |> Async.RunSynchronously
    with
    | :? ArguException as ex ->
        eprintfn "%s" ex.Message
        1
    | ex ->
        eprintfn "%A" ex
        1
