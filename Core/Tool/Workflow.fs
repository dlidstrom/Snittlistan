[<RequireQualifiedAccessAttribute>]
module Workflow

let bitsApi = BitsApi.functions()

let run matchId =
    let matchScores =
        bitsApi.getMatchResults matchId

    let matchResults =
        bitsApi.getMatchResults matchId

    matchScores, matchResults
