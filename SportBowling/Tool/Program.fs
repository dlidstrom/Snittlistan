// dotnet watch run --no-restore -- --api-key 123 --season-id 2006

open System
open Argu

type Arguments =
    | [<CliPrefix(CliPrefix.None)>] Fetch_Matches of ParseResults<FetchMatchesArguments>
    | [<CliPrefix(CliPrefix.None)>] Migrate_Database of ParseResults<MigrateDatabaseArguments>
    | [<CliPrefix(CliPrefix.None)>] Scratch of ParseResults<ScratchArguments>
    | Debug_Http
    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Fetch_Matches _ -> "Fetch matches."
            | Migrate_Database _ -> "Migrate database."
            | Debug_Http -> "Debug HTTP."
            | Scratch _ -> "Scratch program."
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
    | [<Mandatory>] Host of host : string
    | [<Mandatory>] Database of database : string
    | [<Mandatory>] Username of username : string
    | [<Mandatory>] Password of password : string
    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Host _ -> "Specifies the database host."
            | Database _ -> "Specifies the database name."
            | Username _ -> "Specifies the username"
            | Password _ -> "Specifies the password."
and ScratchArguments =
    | Verbose
    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Verbose -> "Verbose output."

let fetchMatches (args : ParseResults<FetchMatchesArguments>) =
    let apiKey = args.GetResult Api_Key
    let seasonId = args.GetResult Season_Id
    let noCheckCertificate =
        if args.Contains(No_Check_Certificate) then Some true else None
    let proxy = args.TryGetResult(Proxy)
    let bitsClient = Api.Bits.Client(apiKey, noCheckCertificate, proxy)
    let workflow = Workflows.FetchMatches()
    workflow.Run bitsClient (Domain.SeasonId seasonId)

let migrateDatabase (args : ParseResults<MigrateDatabaseArguments>) =
    let host = args.GetResult Host
    let database = args.GetResult Database
    let username = args.GetResult Username
    let password = args.GetResult Password
    let workflow = Workflows.MigrateDatabase()
    workflow.Run host database username password

module Scratch =
    module HopacIntro =
        open Hopac
        open Hopac.Infixes

        type Timing(s : string, ?formatEnd : float -> string) =
            do printfn $"%s{s}"
            let start = DateTime.Now
            let defaultEndFormatter t = $"end: %.3f{t}s"
            let endFormatter =
                Option.defaultValue
                    defaultEndFormatter
                    formatEnd
            interface IDisposable with
                member _.Dispose() =
                    let totalSeconds = (DateTime.Now - start).TotalSeconds
                    let s = endFormatter totalSeconds
                    printfn $"%s{s}"

        type JobStatus =
            | Started of jobId : int
            | Completed of jobId : int

        type Product = {
            Id : int
            Name : string
        }

        type Review = {
            ProductId : int
            Author : string
            Comment : string
        }

        type ProductWithReviews = {
            Id : int
            Name : string
            Reviews : (string * string) list
        }

        let getProductReviews id = job {

            // Delay in the place of an external HTTP API call
            do! timeOutMillis 3000

            return [
                {ProductId = id; Author = "John"; Comment = "It's awesome!"}
                {ProductId = id; Author = "Sam"; Comment = "Great product"}
            ]
        }

        let getProduct id = job {
            do! timeOutMillis 2000
            return {
                Id = id
                Name = "My Awesome Product"
            }
        }

        let getProductWithReviews id = job {
            let! product = getProduct id // 1
            let! reviews = getProductReviews id // 2
            return {  // 3
                Id = id
                Name = product.Name
                Reviews = reviews |> List.map (fun r -> r.Author, r.Comment)
            }
        }

        let getProductWithReviews2 id = job {
            let! product, reviews =
                getProduct id <*> getProductReviews id // 2
            return {  // 3
                Id = id
                Name = product.Name
                Reviews = reviews |> List.map (fun r -> r.Author, r.Comment)
            }
        }

        let createJob channel jobId = job {
            do! Ch.give channel (Started jobId)
            do! timeOutMillis (jobId * 1000)
            do! Ch.give channel (Completed jobId)
        }

        let printerJob channel = job {
            let! status = Ch.take channel
            match status with
            | Started jobId ->
                printfn $"starting job: %d{jobId}"
            | Completed jobId ->
                printfn $"completed job: %d{jobId}"
        }

        let Run (_ : ParseResults<ScratchArguments>) =

            use _t = new Timing("start")
            // let longerHelloWorldJob = job {
            //   do! timeOutMillis 2000
            //   printfn "Hello, World!"
            // }
            // run longerHelloWorldJob

            // let jobs = [
            //     createJob 1 4000
            //     createJob 2 3000
            //     createJob 3 2000
            // ]

            // run (Job.conIgnore jobs)

            // run (getProductWithReviews2 1) |> ignore

            let result = run <| job {
                let channel = Ch<JobStatus>()
                let statusPrinter = printerJob channel
                do! Job.foreverServer statusPrinter
                let myJobs = List.init 5 (createJob channel)
                return! Job.conIgnore myJobs
            }

            0

    module ApiGateway =
        open Hopac
        open FSharp.Data
        open HttpFs.Client

        type UserTypeProvider = JsonProvider<"https://api.github.com/users/dlidstrom", RootName = "UserProfile">
        type User = UserTypeProvider.UserProfile

        let httpGet : string -> Job<string> =
            fun url ->
                Request.createUrl Get url
                |> Request.setHeader (UserAgent "FsHopac")
                |> getResponse
                |> Job.bind Response.readBodyAsString

        let basePath = "https://api.github.com"

        let getUser : string -> Job<User> =
            fun username ->
                $"%s{basePath}/users/%s{username}"
                |> httpGet
                |> Job.map UserTypeProvider.Parse

        let Run _ =
            let userProfile = getUser "dlidstrom" |> run
            printfn "%A" userProfile
            0

let run argv =
    let parser = ArgumentParser.Create<Arguments>(
                    programName = AppDomain.CurrentDomain.FriendlyName)
    let results = parser.ParseCommandLine argv

    let debugHttp = results.Contains(Debug_Http)
    let _l =
        if debugHttp then Some (new Infrastructure.LoggingEventListener()) else None

    match results.GetSubCommand() with
    | Fetch_Matches args -> fetchMatches args
    | Migrate_Database args -> migrateDatabase args
    | Scratch args -> Scratch.ApiGateway.Run args
    | Debug_Http -> failwith "Unexpected"

[<EntryPoint>]
let main argv =
    try
        run argv
    with
        | :? ArguException as ex ->
            eprintfn "%s" ex.Message
            1
        | ex ->
            eprintfn "%A" ex
            1
