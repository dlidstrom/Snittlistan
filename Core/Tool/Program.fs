// Learn more about F# at http://fsharp.org

open System
open Argu
open Contracts

module BitsFetch =

    type Arguments =
        | [<Mandatory>] MatchId of matchId : int
        with
            interface IArgParserTemplate with
                member this.Usage =
                    match this with
                    | MatchId _ -> "specifies the match id"

    let main argv =
        let parser = ArgumentParser.Create<Arguments>(programName = "Tool.exe")
        let results = parser.Parse argv
        let matchId = results.GetResult(MatchId)
        let x, y = Workflow.run matchId
        printfn "%s" x.Series.[0].Boards.[0].Scores.[0].PlayerName
        0

[<EntryPoint>]
let main argv =
    // mål: skapa en struktur som går att skicka till API:t
    //
    let x = {
        TeamScore = 1
        OpponentScore = 1
        Turn = 20
        Series = 0
        OpponentSeries = 0
    }
    try
        BitsFetch.main argv
    with
        | :? ArguException as ex ->
            eprintfn "%s" ex.Message
            1
        | ex ->
            eprintfn "%A" ex
            1
