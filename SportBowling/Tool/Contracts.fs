module Contracts

type Undef = exn

module Bits =
    open FSharp.Data
    type MatchResults = JsonProvider<"MatchResults.json">
    type MatchScores = JsonProvider<"MatchScores.json">

module Snittlistan =
    type MatchDto = {
        TeamScore : int
        OpponentScore : int
        Turn : int
        Series : int
        OpponentSeries : int
    }
