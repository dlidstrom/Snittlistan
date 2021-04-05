module Api

open FSharp.Data.HttpRequestHeaders

module Bits =
    type IClient =
        abstract member GetMatchScores : Domain.MatchId -> Async<Contracts.Bits.MatchScores.MatchScore>
        abstract member GetMatchResults : Domain.MatchId -> Domain.MatchSchemeId -> Async<Contracts.Bits.MatchResults.MatchResult>
        abstract member GetDivision : Domain.SeasonId -> Async<Contracts.Bits.Divisions.Division array>
        abstract member GetMatch : Domain.DivisionId -> Domain.SeasonId -> Async<Contracts.Bits.Match.Match array>
        abstract member GetHeadInfo : Domain.MatchId -> Async<Contracts.Bits.HeadInfo.HeadInfo>

    type Client(apiKey, http : Contracts.RequestStringAsync) =

        [<Literal>]
        let root = "https://api.swebowl.se/api/v1"

        let defaultRequest : Contracts.RequestDefinition = {
            Url = ""
            Method = Contracts.Method.Get
            Headers = [
                Referer "https://bits.swebowl.se"
                ContentType "application/json"
            ]
            Body = None
        }

        interface IClient with

            /// https://api.swebowl.se/api/v1/matchResult/GetMatchScores
            member _.GetMatchScores (Domain.MatchId matchId) =
                let matchScoresResponse = async {
                    let! r = http {
                        defaultRequest with
                            Url = $"%s{root}/matchResult/GetMatchScores?APIKey=%s{apiKey}&matchId=%d{matchId}"
                    }
                    return r |> Contracts.Bits.MatchScores.Parse
                }
                matchScoresResponse

            /// https://api.swebowl.se/api/v1/matchResult/GetMatchResults
            member _.GetMatchResults (Domain.MatchId matchId) (Domain.MatchSchemeId matchSchemeId) =
                let matchResultsResponse = async {
                    let! r = http {
                        defaultRequest with
                            Url = $"%s{root}/matchResult/GetMatchResults?APIKey=%s{apiKey}&matchSchemeId=%s{matchSchemeId}&matchId=%d{matchId}"
                    }
                    return r |> Contracts.Bits.MatchResults.Parse
                }
                matchResultsResponse

            /// https://api.swebowl.se/api/v1/Division
            member _.GetDivision (Domain.SeasonId seasonId) =
                let divisionResponse = async {
                    let! r = http {
                        defaultRequest with
                            Url = $"%s{root}/Division?APIKey=%s{apiKey}&teamId=0&countyId=-1&seasonId=%d{seasonId}"
                    }
                    return r |> Contracts.Bits.Divisions.Parse
                }
                divisionResponse

            /// https://api.swebowl.se/api/v1/Match
            member _.GetMatch (Domain.DivisionId divisionId) (Domain.SeasonId seasonId) =
                let matchResponse = async {
                    let! r = http {
                        defaultRequest with
                            Url = $"%s{root}/Match/?APIKey=%s{apiKey}&divisionId=%d{divisionId}&seasonId=%d{seasonId}"
                    }
                    return r |> Contracts.Bits.Match.Parse
                }
                matchResponse

            /// https://api.swebowl.se/api/v1/matchResult/GetHeadInfo
            member _.GetHeadInfo (Domain.MatchId matchId) =
                let headInfoResponse = async {
                    let! r = http {
                        defaultRequest with
                            Url = $"%s{root}/matchResult/GetHeadInfo?APIKey=%s{apiKey}&id=%d{matchId}"
                    }
                    return r |> Contracts.Bits.HeadInfo.Parse
                }
                headInfoResponse
