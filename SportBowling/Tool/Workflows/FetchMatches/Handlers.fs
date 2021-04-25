module Handlers

type Handler(logger: Contracts.Logger, bitsClient: Api.Bits.IClient) =
    member this.handle =
        function
        | Operations.FetchDivision seasonId -> this.fetchDivision seasonId
        | Operations.FetchMatchesForDivision (seasonId, divisionId) ->
            this.fetchMatchesForDivision seasonId divisionId
        | Operations.Matches matches -> this.handleMatches matches

    member this.handleMatches matches = async { return [] }

    member this.fetchDivision(seasonId: Domain.SeasonId) =
        async {
            let (Domain.SeasonId i) = seasonId
            logger.Log $"fetch division %d{i}"

            let! divisions = bitsClient.GetDivision seasonId

            return
                divisions
                |> Seq.map
                    (fun division ->
                        Operations.FetchMatchesForDivision(
                            seasonId,
                            Domain.DivisionId division.DivisionId
                        ))
                |> Seq.toList
        }

    member this.fetchMatchesForDivision seasonId divisionId =
        async {
            let (Domain.SeasonId s, Domain.DivisionId d) = (seasonId, divisionId)
            logger.Log $"fetch matches for season %d{s} division %d{d}"

            let! matches = bitsClient.GetMatch divisionId seasonId
            return [ Operations.Matches matches ]
        }
