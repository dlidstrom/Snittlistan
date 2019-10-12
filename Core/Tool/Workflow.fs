[<RequireQualifiedAccessAttribute>]
module Workflow

let run (bitsClient : Api.Bits.Client) matchId =
    let matchScores =
        bitsClient.GetMatchScores matchId

    let matchResults =
        bitsClient.GetMatchResults matchId

    matchScores, matchResults
