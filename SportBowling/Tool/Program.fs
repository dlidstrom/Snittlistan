// dotnet watch run --no-restore -- --apikey 123 --seasonid 2006
open Argu
open System

type Arguments =
    | [<Mandatory>] ApiKey of apiKey : string
    | [<Mandatory>] SeasonId of seasonId : int
    with
        interface IArgParserTemplate with
            member this.Usage =
                match this with
                | ApiKey _ -> "Specifies the ApiKey"
                | SeasonId _ -> "specifies the season id"

let run argv =
    let parser = ArgumentParser.Create<Arguments>(programName = AppDomain.CurrentDomain.FriendlyName)
    let results = parser.Parse argv
    let apiKey = results.GetResult(ApiKey)
    let seasonId = results.GetResult(SeasonId)
    let bitsClient = Api.Bits.Client(apiKey)
    FetchMatches.run bitsClient seasonId

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
