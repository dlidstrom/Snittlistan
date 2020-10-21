[<RequireQualifiedAccessAttribute>]
module Api

open FSharp.Data
open FSharp.Data.HttpRequestHeaders
open Contracts

let private buildMatchScoresUrl apiKey matchId =
    let root = "https://api.swebowl.se/api/v1/matchResult/GetMatchScores"
    let url = sprintf "%s?APIKey=%s&matchId=%d" root apiKey matchId
    url

let private buildMatchResultsUrl apiKey matchId =
    let root = "https://api.swebowl.se/api/v1/matchResult/GetMatchResults"
    let url = sprintf "%s?APIKey=%s&matchSchemeId=8M8BA&matchId=%d" root apiKey matchId
    url

let private buildRefererUrl matchId =
    let root = "https://bits.swebowl.se/match-detail"
    let url = sprintf "%s?matchid=%d" root matchId
    url

module Bits =
    type Client(apiKey) =
        let matchScoresUrl =
            buildMatchScoresUrl apiKey
        let matchResultsUrl =
            buildMatchResultsUrl apiKey

        member _.GetMatchScores matchId =
            let url = matchScoresUrl matchId
            let referer = buildRefererUrl matchId
            let matchScores =
                Http.RequestString
                    (url = url, httpMethod = HttpMethod.Get,
                     headers = [
                        Referer referer
                    ])
                |> Bits.MatchScores.Parse
            matchScores

        member _.GetMatchResults matchId =
            let matchResultsUrl = matchResultsUrl matchId
            let referer = buildRefererUrl matchId
            let matchResults =
                Http.RequestString
                    (url = matchResultsUrl, httpMethod = HttpMethod.Get,
                     headers = [
                        Referer referer
                    ])
                |> Bits.MatchResults.Parse
            matchResults
