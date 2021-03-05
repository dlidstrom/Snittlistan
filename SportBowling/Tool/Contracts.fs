module Contracts

open FSharp.Data

module Bits =
    type MatchResults = JsonProvider<"Json/MatchResults.json", InferTypesFromValues = true, RootName = "MatchResult">
    type MatchScores = JsonProvider<"Json/MatchScores.json", InferTypesFromValues = true, RootName = "MatchScore">

    /// [
    ///   {
    ///     "divisionId": "1",
    ///     "divisionName": "Elitserien Herrar"
    ///   }
    /// ]
    type Divisions = JsonProvider<"Json/Division.json", InferTypesFromValues = true, RootName = "Division">
    type Match = JsonProvider<"Json/Match.json", InferTypesFromValues = true, RootName = "Match">
    type HeadInfo = JsonProvider<"Json/HeadInfo.json", InferTypesFromValues = true, RootName = "HeadInfo">
