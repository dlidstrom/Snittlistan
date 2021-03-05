// dotnet watch run --no-restore -- --api-key 123 --season-id 2006

open System
open Argu
open DbUp.Engine

type Arguments =
    | [<CliPrefix(CliPrefix.None)>] Fetch_Matches of ParseResults<FetchMatchesArguments>
    | [<CliPrefix(CliPrefix.None)>] Migrate_Database of ParseResults<MigrateDatabaseArguments>
    | [<Mandatory>] Host of host : string
    | [<Mandatory>] Database of database : string
    | [<Mandatory>] Username of username : string
    | [<Mandatory>] Password of password : string
    | Debug_Http
    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Fetch_Matches _ -> "Fetch matches."
            | Migrate_Database _ -> "Migrate database."
            | Host _ -> "Specifies the database host."
            | Database _ -> "Specifies the database name."
            | Username _ -> "Specifies the username"
            | Password _ -> "Specifies the password."
            | Debug_Http -> "Debug HTTP."
and FetchMatchesArguments =
    | [<Mandatory>] Api_Key of apiKey : string
    | [<Mandatory>] Season_Id of seasonId : int
    | No_Check_Certificate
    | Proxy of proxy : string
    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Api_Key _ -> "Specifies the ApiKey."
            | Season_Id _ -> "specifies the season id."
            | No_Check_Certificate -> "Don't check the server certificate against the available certificate authorities."
            | Proxy _ -> "Specifies the proxy to use."
and MigrateDatabaseArguments =
    | Timeout of timeout : int
    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Timeout _ -> "Timeout in seconds"
and ScratchArguments =
    | Verbose
    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Verbose -> "Verbose output."

let fetchMatches (args : ParseResults<FetchMatchesArguments>) connection =
    let apiKey = args.GetResult Api_Key
    let seasonId = args.GetResult Season_Id
    let noCheckCertificate =
        if args.Contains(No_Check_Certificate) then Some true else None
    let proxy = args.TryGetResult(Proxy)
    let bitsClient = Api.Bits.Client(apiKey, noCheckCertificate, proxy)
    let workflow = Workflows.FetchMatches(Database.Gateway(fun () -> new Database.Context(connection)))
    workflow.Run bitsClient (Domain.SeasonId seasonId)

let migrateDatabase connection =
    let confirmAction (lst : ResizeArray<SqlScript>) =
        printfn "Scripts to execute:"
        Seq.map (fun (it : SqlScript) -> it.Name) lst |> Seq.iter (printfn "%s")
        printfn "Enter [y] to accept"
        Console.ReadLine() = "y"
    let workflow = Workflows.MigrateDatabase(confirmAction)
    workflow.Run connection

let run argv =
    let parser = ArgumentParser.Create<Arguments>(
                    programName = AppDomain.CurrentDomain.FriendlyName)
    let results = parser.ParseCommandLine argv

    let host = results.GetResult Host
    let database = results.GetResult Database
    let username = results.GetResult Username
    let password = results.GetResult Password
    let connection : Database.DatabaseConnection = {
        Host = host
        Database = database
        Username = username
        Password = password
    }
    let debugHttp = results.Contains(Debug_Http)
    let _l =
        if debugHttp then Some (new Infrastructure.LoggingEventListener()) else None

    match results.GetSubCommand() with
    | Fetch_Matches args -> fetchMatches args connection
    | Migrate_Database _ -> migrateDatabase connection
    | Debug_Http -> failwith "Unexpected"
    | Host _ -> failwith "Unexpected"
    | Database _ -> failwith "Unexpected"
    | Username _ -> failwith "Unexpected"
    | Password _ -> failwith "Unexpected"

[<EntryPoint>]
let main argv =
    try
        match run argv with
        | Some ex -> raise ex
        | None -> 0
    with
        | :? ArguException as ex ->
            eprintfn "%s" ex.Message
            1
        | ex ->
            eprintfn "%A" ex
            1
