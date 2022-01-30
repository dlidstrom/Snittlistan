using Newtonsoft.Json;

#nullable enable

namespace Snittlistan.Web.Infrastructure.Bits.Contracts;
public class MatchRound
{
    [JsonProperty("matchRoundId")]
    public int MatchRoundId { get; set; }

    [JsonProperty("matchDate")]
    public string? MatchDate { get; set; }

    [JsonProperty("matchTime")]
    public int MatchTime { get; set; }

    [JsonProperty("matchDateTime")]
    public string? MatchDateTime { get; set; }

    [JsonProperty("matchId")]
    public int MatchId { get; set; }

    [JsonProperty("matchHomeTeamId")]
    public int MatchHomeTeamId { get; set; }

    [JsonProperty("matchHomeTeamName")]
    public string? MatchHomeTeamName { get; set; }

    [JsonProperty("matchHomeTeamAlias")]
    public string? MatchHomeTeamAlias { get; set; }

    [JsonProperty("matchAwayTeamId")]
    public int MatchAwayTeamId { get; set; }

    [JsonProperty("matchAwayTeamName")]
    public string? MatchAwayTeamName { get; set; }

    [JsonProperty("matchAwayTeamAlias")]
    public string? MatchAwayTeamAlias { get; set; }

    [JsonProperty("matchHomeTeamScore")]
    public int MatchHomeTeamScore { get; set; }

    [JsonProperty("matchAwayTeamScore")]
    public int MatchAwayTeamScore { get; set; }

    [JsonProperty("matchHomeTeamResult")]
    public int MatchHomeTeamResult { get; set; }

    [JsonProperty("matchAwayTeamResult")]
    public int MatchAwayTeamResult { get; set; }

    [JsonProperty("matchOilPatternId")]
    public int MatchOilPatternId { get; set; }

    [JsonProperty("matchOilPatternName")]
    public string? MatchOilPatternName { get; set; }

    [JsonProperty("matchHallId")]
    public int MatchHallId { get; set; }

    [JsonProperty("matchHallName")]
    public string? MatchHallName { get; set; }

    [JsonProperty("matchHallCity")]
    public string? MatchHallCity { get; set; }

    [JsonProperty("matchDivisionId")]
    public int MatchDivisionId { get; set; }

    [JsonProperty("matchSeason")]
    public int MatchSeason { get; set; }

    [JsonProperty("matchDateOld")]
    public DateTimeOffset MatchDateOld { get; set; }

    [JsonProperty("matchTimeOld")]
    public int MatchTimeOld { get; set; }

    [JsonProperty("matchStatus")]
    public int MatchStatus { get; set; }

    [JsonProperty("matchVsTeams")]
    public string? MatchVsTeams { get; set; }

    [JsonProperty("matchVsResult")]
    public string? MatchVsResult { get; set; }

    [JsonProperty("matchHasBeenPlayed")]
    public bool MatchHasBeenPlayed { get; set; }

    [JsonProperty("matchAlleyGroupName")]
    public string? MatchAlleyGroupName { get; set; }

    [JsonProperty("matchDivisionName")]
    public string? MatchDivisionName { get; set; }

    [JsonProperty("matchLeagueName")]
    public string? MatchLeagueName { get; set; }

    [JsonProperty("matchLeagueId")]
    public int MatchLeagueId { get; set; }

    [JsonProperty("matchNbrOfLanes")]
    public int MatchNbrOfLanes { get; set; }

    [JsonProperty("matchNbrOfPlayers")]
    public int MatchNbrOfPlayers { get; set; }

    [JsonProperty("matchAllot")]
    public bool MatchAllot { get; set; }

    [JsonProperty("matchFinished")]
    public bool MatchFinished { get; set; }

    [JsonProperty("matchSchemeId")]
    public string? MatchSchemeId { get; set; }

    [JsonProperty("matchSchemeNbrOfLanes")]
    public int MatchSchemeNbrOfLanes { get; set; }

    [JsonProperty("matchSchemeNbrOfPlayers")]
    public int MatchSchemeNbrOfPlayers { get; set; }

    [JsonProperty("matchDivisionSeasonHcpNettoOrBrutto")]
    public bool MatchDivisionSeasonHcpNettoOrBrutto { get; set; }

    [JsonProperty("matchIsInNationalLeague")]
    public bool MatchIsInNationalLeague { get; set; }

    [JsonProperty("matchLeagueSeasonLevelRankType")]
    public string? MatchLeagueSeasonLevelRankType { get; set; }

    [JsonProperty("matchStrikeOut")]
    public bool MatchStrikeOut { get; set; }

    [JsonProperty("matchStrikeOutNbrOfRounds")]
    public int MatchStrikeOutNbrOfRounds { get; set; }

    [JsonProperty("matchDivisionSeasonNbrOfLanePoints")]
    public int MatchDivisionSeasonNbrOfLanePoints { get; set; }

    [JsonProperty("matchDivisionSeasonNbrOfBonusPoints")]
    public int MatchDivisionSeasonNbrOfBonusPoints { get; set; }

    [JsonProperty("matchIsUsingLanePoints")]
    public bool MatchIsUsingLanePoints { get; set; }

    [JsonProperty("matchHcp")]
    public int MatchHcp { get; set; }

    [JsonProperty("matchLevelId")]
    public int MatchLevelId { get; set; }

    [JsonProperty("matchDivisionSeasonRankType")]
    public string? MatchDivisionSeasonRankType { get; set; }

    [JsonProperty("matchDivisionSeasonLanePoints")]
    public bool MatchDivisionSeasonLanePoints { get; set; }

    [JsonProperty("matchCupId")]
    public object? MatchCupId { get; set; }

    [JsonProperty("matchFinishedHomeTeam")]
    public bool MatchFinishedHomeTeam { get; set; }

    [JsonProperty("matchFinishedAwayTeam")]
    public bool MatchFinishedAwayTeam { get; set; }

    [JsonProperty("matchHcpTypeId")]
    public object? MatchHcpTypeId { get; set; }

    [JsonProperty("matchHcpTypeDescription")]
    public object? MatchHcpTypeDescription { get; set; }

    [JsonProperty("matchDivisionSeasonAverageFrom")]
    public int MatchDivisionSeasonAverageFrom { get; set; }

    [JsonProperty("matchDivisionSeasonAverageTo")]
    public int MatchDivisionSeasonAverageTo { get; set; }

    [JsonProperty("matchDivisionSeasonMaxHcp")]
    public int MatchDivisionSeasonMaxHcp { get; set; }

    [JsonProperty("matchDivisionSeasonPercent")]
    public int MatchDivisionSeasonPercent { get; set; }

    [JsonProperty("homeTeamClubId")]
    public int HomeTeamClubId { get; set; }

    [JsonProperty("awayTeamClubId")]
    public int AwayTeamClubId { get; set; }

    [JsonProperty("allowOilProfileUpdationTillDate")]
    public DateTimeOffset AllowOilProfileUpdationTillDate { get; set; }

    [JsonProperty("matchOilProfile")]
    public MatchOilProfile? MatchOilProfile { get; set; }

    [JsonProperty("matchHallOnlineScoringUrl")]
    public string? MatchHallOnlineScoringUrl { get; set; }

    [JsonProperty("currentDate")]
    public DateTimeOffset CurrentDate { get; set; }
}
