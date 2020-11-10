module FetchMatches

let run (bitsClient : Api.Bits.Client) seasonId =
    (*

        ERROR CHECKING ALL RESPONSES
        - Max length of arrays (500)
        - Custom data types for each property

        STORE ALL RESPONSES
        - use tag with response, so that workflow can be resumed if interrupted,
          for example, use the date as the default tag (overridable if necessary)

        SINGLE INSTANCE
        - only allow a single instance to run (mutex)

    1. POST https://api.swebowl.se/api/v1/Division?APIKey=___&teamId=0&countyId=-1&seasonId=2006
        GetDivision (all) - from 2006 ->
        {"search":"","take":500,"skip":0,"page":1,"pageSize":500,"sort":[{"field":"clubName","dir":"asc"}]}
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

    0
