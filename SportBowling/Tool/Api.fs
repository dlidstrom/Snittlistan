module Api

open FSharp.Data
open FSharp.Data.HttpRequestHeaders

module Bits =
    type Client(apiKey) =
        [<Literal>]
        let root = "https://api.swebowl.se/api/v1"

        let performRequest url httpMethod =
            Http.RequestString
                (url = url, httpMethod = httpMethod,
                 headers = [
                    Referer "https://bits.swebowl.se"
                ])

        member _.GetMatchScores matchId =
            let url = $"%s{root}/matchResult/GetMatchScores?APIKey=%s{apiKey}&matchId=%d{matchId}"
            let matchScores =
                performRequest url HttpMethod.Get
                |> Contracts.Bits.MatchScores.Parse
            matchScores

        member _.GetMatchResults matchId matchSchemeId =
            let url = $"%s{root}/matchResult/GetMatchResults?APIKey=%s{apiKey}&matchSchemeId=%s{matchSchemeId}&matchId=%d{matchId}"
            let matchResults =
                performRequest url HttpMethod.Get
                |> Contracts.Bits.MatchResults.Parse
            matchResults

        // member _.GetDivisions ...
        (*
                1. POST https://api.swebowl.se/api/v1/Division?APIKey=___&teamId=0&countyId=-1&seasonId=2006
        GetDivision (all) - from 2006 ->
        {"search":"","take":500,"skip":0,"page":1,"pageSize":500,"sort":[{"field":"clubName","dir":"asc"}]}

        *)
