namespace Workflows
open System.Collections.Generic

type Operation =
    | FetchDivision of seasonId : Domain.SeasonId
    | FetchMatchesForDivision of divisionId : Domain.DivisionId

type FetchMatches(bitsClient : Api.Bits.IClient) =

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

    let fetchDivision seasonId = async {
        let! divisions = bitsClient.GetDivision seasonId

        return divisions
               |> Seq.map (fun d -> FetchMatchesForDivision (Domain.DivisionId d.DivisionId))
               |> Seq.toList
               |> Some
    }

    let fetchMatchesForDivision (Domain.DivisionId divisionId) = async {
        printfn "Fetching for %d" divisionId
        return None
    }

    let handle = function
        | FetchDivision seasonId ->
            fetchDivision seasonId
        | FetchMatchesForDivision divisionId ->
            fetchMatchesForDivision divisionId

    let handleOperationResult (stack : Stack<Operation>) = function
        | Some operations -> operations |> List.iter stack.Push
        | None -> ()

    let rec processStack (stack : Stack<Operation>) =
        match stack.TryPop() with
        | (true, currentOperation) ->
            async {
                let! r = handle currentOperation
                r |> handleOperationResult stack
                do! processStack stack
            }
        | (false, _) ->
            async {
                return ()
            }

    member _.Run seasonId = async {
            let operationStack = Stack<Operation>()
            operationStack.Push(FetchDivision seasonId)
            do! processStack operationStack
        }

        // get response from bits.response table first, check if recent
        // if so, use stored response, otherwise fetch new, then continue
        // recentness may need to be a configuration
        // use config files to specify program configuration, instead of
        // only commandline

        // database queries are discriminated union
        // requests are discriminated union, log request type (DU case type)

        // use this instead of EF?
        // https://github.com/Zaid-Ajaj/Npgsql.FSharp.Analyzer
        (*
            use connection = new NpgsqlConnection("YOUR CONNECTION STRING")
            connection.Open()

let users =
    connection
    |> Sql.existingConnection
    |> Sql.query "SELECT * FROM users"
    |> Sql.execute (fun read ->
        {
            Id = read.int "user_id"
            FirstName = read.text "first_name"
        })
        *)

        // store all divisions
        // for division in divisions do
        //     let matches = bitsClient.GetMatch (Domain.DivisionId division.DivisionId) seasonId
        //     for matchItem in matches do
        //         let headInfo = bitsClient.GetHeadInfo (Domain.MatchId matchItem.MatchId)
        //         printfn "%s" headInfo.MatchSchemeId
        //         // let matchScheme = bitsClient.GetHeadResultInfo headInfo.MatchScheme
        //         // Some (exn "Break here")
