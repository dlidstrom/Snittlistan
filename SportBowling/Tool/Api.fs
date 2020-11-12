module Api

open System.Net
open FSharp.Data
open FSharp.Data.HttpRequestHeaders
open FSharp.Json

module Bits =
    type Client(apiKey, noCheckCertificate : bool option, proxy : string option) =
        [<Literal>]
        let root = "https://api.swebowl.se/api/v1"
        let customizeRequest (req : HttpWebRequest) =
            proxy |> function
            | Some s -> req.Proxy <- WebProxy(s, true)
            | _ -> ()
            noCheckCertificate |> function
            | Some b when b ->
                req.ServerCertificateValidationCallback <-
                    fun _sender _certificate _chain _policyErrors -> true
            | _ -> ()
            req

        let performGet url =
            Http.RequestString(
                url = url,
                httpMethod = HttpMethod.Get,
                headers = [
                    Referer "https://bits.swebowl.se"
                ],
                customizeHttpRequest = customizeRequest)

        let performPost url body =
            Http.RequestString(
                url = url,
                httpMethod = HttpMethod.Post,
                body = TextRequest (Json.serialize body),
                headers = [
                    Referer "https://bits.swebowl.se"
                    ContentType "application/json"
                ],
                customizeHttpRequest = customizeRequest)

        member _.GetMatchScores matchId =
            let matchScores =
                performGet $"%s{root}/matchResult/GetMatchScores?APIKey=%s{apiKey}&matchId=%d{matchId}"
                |> Contracts.Bits.MatchScores.Parse
            matchScores

        member _.GetMatchResults matchId matchSchemeId =
            let matchResults =
                performGet $"%s{root}/matchResult/GetMatchResults?APIKey=%s{apiKey}&matchSchemeId=%s{matchSchemeId}&matchId=%d{matchId}"
                |> Contracts.Bits.MatchResults.Parse
            matchResults

        member _.GetDivisions seasonId =
            let divisions =
                performGet
                    $"%s{root}/Division?APIKey=%s{apiKey}&teamId=0&countyId=-1&seasonId=%d{seasonId}"
                |> Contracts.Bits.Divisions.Parse
            divisions
