module Contracts

module Bits =
    open FSharp.Data
    type MatchResults = JsonProvider<"Json/MatchResults.json">
    type MatchScores = JsonProvider<"Json/MatchScores.json">
    type Divisions = JsonProvider<"Json/Divisions.json">
