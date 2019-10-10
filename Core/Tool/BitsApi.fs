[<RequireQualifiedAccessAttribute>]
module BitsApi

open System
open FSharp.Data
open FSharp.Data.HttpRequestHeaders
open Contracts

let apiKeyEnvVar = "BITS_API_KEY"

let private apiKey =
    Environment.GetEnvironmentVariable(variable = apiKeyEnvVar)

let private buildMatchScoresUrl matchId =
    let root = "https://api.swebowl.se/api/v1/matchResult/GetMatchScores"
    let url = sprintf "%s?APIKey=%s&matchId=%d" root apiKey matchId
    url

let private buildMatchResultsUrl matchId =
    let root = "https://api.swebowl.se/api/v1/matchResult/GetMatchResults"
    let url = sprintf "%s?APIKey=%s&matchSchemeId=8M8BA&matchId=%d" root apiKey matchId
    url

let private buildRefererUrl matchId =
    let root = "https://bits.swebowl.se/match-detail"
    let url = sprintf "%s?matchid=%d" root matchId
    url

let getMatchScores matchId =
    let url = buildMatchScoresUrl matchId
    let referer = buildRefererUrl matchId
    let matchScores =
        Http.RequestString
            (url = url, httpMethod = HttpMethod.Get,
             headers = [
                Referer referer
            ])
        |> Bits.MatchScores.Parse
    matchScores

let getMatchResults matchId =
    let matchResultsUrl = buildMatchResultsUrl matchId
    let referer = buildRefererUrl matchId
    let matchResults =
        Http.RequestString
            (url = matchResultsUrl, httpMethod = HttpMethod.Get,
             headers = [
                Referer referer
            ])
        |> Bits.MatchResults.Parse
    matchResults

let functions() = {|
    getMatchScores = getMatchScores
    getMatchResults = getMatchResults
|}
