module Operations

type Operation =
    | FetchDivision of seasonId: Domain.SeasonId
    | FetchMatchesForDivision of seasonId: Domain.SeasonId * divisionId: Domain.DivisionId
    | Matches of matches : Contracts.Bits.Match.Match array
