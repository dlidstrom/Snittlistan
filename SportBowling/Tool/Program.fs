open Argu
open System

type Arguments =
    | [<Mandatory>] ApiKey of apiKey : string
    | [<Mandatory>] MatchId of matchId : int
    with
        interface IArgParserTemplate with
            member this.Usage =
                match this with
                | ApiKey _ -> "Specifies the ApiKey"
                | MatchId _ -> "specifies the match id"

let run argv =
    let parser = ArgumentParser.Create<Arguments>(programName = AppDomain.CurrentDomain.FriendlyName)
    let results = parser.Parse argv
    let apiKey = results.GetResult(ApiKey)
    let matchId = results.GetResult(MatchId)
    let bitsClient = Api.Bits.Client(apiKey)
    let x, y = Workflow.run bitsClient matchId
    printfn "%s" x.Series.[0].Boards.[0].Scores.[0].PlayerName
    0

[<EntryPoint>]
let main argv =
    try
        FetchMatches.run argv
    with
        | :? ArguException as ex ->
            eprintfn "%s" ex.Message
            1
        | ex ->
            eprintfn "%A" ex
            1
