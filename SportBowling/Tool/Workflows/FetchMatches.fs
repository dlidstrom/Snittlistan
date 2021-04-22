namespace Workflows

open System.Collections.Generic

type Operation =
    | FetchDivision of seasonId: Domain.SeasonId
    | FetchMatchesForDivision of seasonId: Domain.SeasonId * divisionId: Domain.DivisionId

module private Handlers =
    type Handler(logger: Contracts.Logger, bitsClient: Api.Bits.IClient) =
        member this.handle =
            function
            | FetchDivision seasonId -> this.fetchDivision seasonId
            | FetchMatchesForDivision (seasonId, divisionId) ->
                this.fetchMatchesForDivision seasonId divisionId

        member this.fetchDivision(seasonId: Domain.SeasonId) =
            let (Domain.SeasonId i) = seasonId
            logger.Log $"fetch division %d{i}"

            async {
                let! divisions = bitsClient.GetDivision seasonId

                return
                    divisions
                    |> Seq.map
                        (fun division ->
                            FetchMatchesForDivision(seasonId, Domain.DivisionId division.DivisionId))
                    |> Seq.toList
                    |> Some
            }

        member this.fetchMatchesForDivision seasonId divisionId =
            let (Domain.SeasonId s, Domain.DivisionId d) = (seasonId, divisionId)
            logger.Log $"fetch matches for season %d{s} division %d{d}"

            async {
                let! matches = bitsClient.GetMatch divisionId seasonId
                return None
            }

type FetchMatches(bitsClient: Api.Bits.IClient, logger: Contracts.Logger) =

    (*

        ERROR CHECKING ALL RESPONSES
        - Max length of arrays (500)
        - Custom data types for each property

        STORE ALL RESPONSES
        - use tag with response, so that workflow can be resumed if interrupted,
          for example, use the date as the default tag (overridable if necessary)

        SINGLE INSTANCE
        - only allow a single instance to run (mutex)

    1. GET https://api.swebowl.se/api/v1/Division?APIKey=___seasonId=2006
        GetDivision (all) - from 2006 ->
    2. for each division in season, get the matches
        GET https://api.swebowl.se/api/v1/Match/?APIKey=___&divisionId=1&seasonId=2009
    3. for each game in division
        GET https://api.swebowl.se/api/v1/matchResult/GetHeadInfo?APIKey=___&id=3211933
        GET https://api.swebowl.se/api/v1/matchResult/GetHeadResultInfo?APIKey=___&id=3211933
        GET https://api.swebowl.se/api/v1/matchResult/GetMatchScores?APIKey=___&matchId=3211933
        GET https://api.swebowl.se/api/v1/matchResult/GetMatchResults?APIKey=___&matchId=3211933&matchSchemeId=8M8BA
            matchSchemeId comes from GetHeadInfo
    4.
    *)

    let rec processStack (handler: Handlers.Handler) (stack: Stack<Operation>) =
        match stack.TryPop() with
        | (true, currentOperation) ->
            async {
                match! handler.handle currentOperation with
                | Some operations -> operations |> List.iter stack.Push
                | None -> ()

                do! processStack handler stack
            }
        | (false, _) -> async { return () }

    member _.Run seasonId =
        let handler = Handlers.Handler(logger, bitsClient)

        async {
            let operationStack = Stack<Operation>()
            operationStack.Push(FetchDivision seasonId)
            do! processStack handler operationStack
            return 0
        }

// get response from bits.response table first, check if recent
// if so, use stored response, otherwise fetch new, then continue
// recentness may need to be a configuration
// use config files to specify program configuration, instead of
// only commandline

// database queries are discriminated union
// requests are discriminated union, log request type (DU case type)

// store all divisions
// for division in divisions do
//     let matches = bitsClient.GetMatch (Domain.DivisionId division.DivisionId) seasonId
//     for matchItem in matches do
//         let headInfo = bitsClient.GetHeadInfo (Domain.MatchId matchItem.MatchId)
//         printfn "%s" headInfo.MatchSchemeId
//         // let matchScheme = bitsClient.GetHeadResultInfo headInfo.MatchScheme
//         // Some (exn "Break here")
