using Newtonsoft.Json;

#nullable enable

namespace Snittlistan.Web.Infrastructure.Bits.Contracts;
public partial class HeadInfo
{
    [JsonProperty("dayOfWeek")]
    public string? DayOfWeek { get; set; }

    [JsonProperty("divisionIsCup")]
    public bool DivisionIsCup { get; set; }

    [JsonProperty("matchId")]
    public int MatchId { get; set; }

    [JsonProperty("standingsURL")]
    public object? StandingsUrl { get; set; }

    [JsonProperty("matchFactURL")]
    public object? MatchFactUrl { get; set; }

    [JsonProperty("hallSchemeURL")]
    public object? HallSchemeUrl { get; set; }

    [JsonProperty("matchTeamHomeVsAway")]
    public object? MatchTeamHomeVsAway { get; set; }

    [JsonProperty("matchDateTime")]
    public object? MatchDateTime { get; set; }

    [JsonProperty("matchEndDateTime")]
    public DateTime MatchEndDateTime { get; set; }

    [JsonProperty("matchIdPrevious")]
    public int MatchIdPrevious { get; set; }

    [JsonProperty("matchLotTemplateRowRoundNbr")]
    public int MatchLotTemplateRowRoundNbr { get; set; }

    [JsonProperty("matchStatus")]
    public int MatchStatus { get; set; }

    [JsonProperty("matchHomeTeamId")]
    public int MatchHomeTeamId { get; set; }

    [JsonProperty("matchHomeClubId")]
    public int MatchHomeClubId { get; set; }

    [JsonProperty("matchAwayClubId")]
    public int MatchAwayClubId { get; set; }

    [JsonProperty("matchHomeTeamTypeType")]
    public object? MatchHomeTeamTypeType { get; set; }

    [JsonProperty("matchAwayTeamTypeType")]
    public object? MatchAwayTeamTypeType { get; set; }

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

    [JsonProperty("matchAlleyGroup")]
    public int MatchAlleyGroup { get; set; }

    [JsonProperty("alleyGroupName")]
    public object? AlleyGroupName { get; set; }

    [JsonProperty("alleyGroupStartLane")]
    public int AlleyGroupStartLane { get; set; }

    [JsonProperty("alleyGroupNbrOfLanes")]
    public int AlleyGroupNbrOfLanes { get; set; }

    [JsonProperty("rankCompetition")]
    public double RankCompetition { get; set; }

    [JsonProperty("matchDate")]
    public string? MatchDate { get; set; }

    [JsonProperty("matchDivisionId")]
    public int MatchDivisionId { get; set; }

    [JsonProperty("divisionRegion")]
    public int DivisionRegion { get; set; }

    [JsonProperty("matchLeagueId")]
    public int MatchLeagueId { get; set; }

    [JsonProperty("matchLevelId")]
    public int MatchLevelId { get; set; }

    [JsonProperty("matchDivisionHcp")]
    public int MatchDivisionHcp { get; set; }

    [JsonProperty("matchDivisionName")]
    public string? MatchDivisionName { get; set; }

    [JsonProperty("matchDivisionShortName")]
    public object? MatchDivisionShortName { get; set; }

    [JsonProperty("matchHallId")]
    public int MatchHallId { get; set; }

    [JsonProperty("matchHallName")]
    public string? MatchHallName { get; set; }

    [JsonProperty("matchRoundId")]
    public int MatchRoundId { get; set; }

    [JsonProperty("matchNbrOfLanes")]
    public int MatchNbrOfLanes { get; set; }

    [JsonProperty("matchNbrOfPlayers")]
    public int MatchNbrOfPlayers { get; set; }

    [JsonProperty("matchSchemeId")]
    public string? MatchSchemeId { get; set; }

    [JsonProperty("matchFinished")]
    public bool MatchFinished { get; set; }

    [JsonProperty("matchTime")]
    public int MatchTime { get; set; }

    [JsonProperty("matchEndTime")]
    public int MatchEndTime { get; set; }

    [JsonProperty("levelPlayTime")]
    public int LevelPlayTime { get; set; }

    [JsonProperty("matchTimeOld")]
    public int MatchTimeOld { get; set; }

    [JsonProperty("matchDateOld")]
    public DateTime MatchDateOld { get; set; }

    [JsonProperty("matchHcp")]
    public int MatchHcp { get; set; }

    [JsonProperty("matchBossGroup")]
    public int MatchBossGroup { get; set; }

    [JsonProperty("matchLanePoints")]
    public bool MatchLanePoints { get; set; }

    [JsonProperty("matchHomeTeamScore")]
    public int MatchHomeTeamScore { get; set; }

    [JsonProperty("matchAwayTeamScore")]
    public int MatchAwayTeamScore { get; set; }

    [JsonProperty("matchHomeTeamResult")]
    public int MatchHomeTeamResult { get; set; }

    [JsonProperty("matchAwayTeamResult")]
    public int MatchAwayTeamResult { get; set; }

    [JsonProperty("matchSeason")]
    public int MatchSeason { get; set; }

    [JsonProperty("licenceAgreementSeason")]
    public int LicenceAgreementSeason { get; set; }

    [JsonProperty("clubAgreementSeason")]
    public int ClubAgreementSeason { get; set; }

    [JsonProperty("teamLevel")]
    public int TeamLevel { get; set; }

    [JsonProperty("matchOmit")]
    public bool MatchOmit { get; set; }

    [JsonProperty("hallIdSbhf")]
    public int HallIdSbhf { get; set; }

    [JsonProperty("matchAllot")]
    public bool MatchAllot { get; set; }

    [JsonProperty("matchLotExtraMatch")]
    public bool MatchLotExtraMatch { get; set; }

    [JsonProperty("matchStrikeOut")]
    public bool MatchStrikeOut { get; set; }

    [JsonProperty("matchStrikeOutNbrOfRounds")]
    public int MatchStrikeOutNbrOfRounds { get; set; }

    [JsonProperty("matchCompetitionLevel")]
    public double MatchCompetitionLevel { get; set; }

    [JsonProperty("matchStrikeOutBool")]
    public bool MatchStrikeOutBool { get; set; }

    [JsonProperty("matchDateTimeChanged")]
    public bool MatchDateTimeChanged { get; set; }

    [JsonProperty("matchHomeTeamVsAwayTeam")]
    public string? MatchHomeTeamVsAwayTeam { get; set; }

    [JsonProperty("matchResult")]
    public string? MatchResult { get; set; }

    [JsonProperty("matchTeams")]
    public string? MatchTeams { get; set; }

    [JsonProperty("homeTeamStartLane")]
    public int HomeTeamStartLane { get; set; }

    [JsonProperty("homeTeamNbrOfLanes")]
    public int HomeTeamNbrOfLanes { get; set; }

    [JsonProperty("homeTeamLaneGroup")]
    public string? HomeTeamLaneGroup { get; set; }

    [JsonProperty("matchHour")]
    public int MatchHour { get; set; }

    [JsonProperty("matchMinute")]
    public int MatchMinute { get; set; }

    [JsonProperty("matchDayFormatted")]
    public string? MatchDayFormatted { get; set; }

    [JsonProperty("matchDayFormattedWithRound")]
    public string? MatchDayFormattedWithRound { get; set; }

    [JsonProperty("matchDayFormattedReportDates")]
    public string? MatchDayFormattedReportDates { get; set; }

    [JsonProperty("regionName")]
    public string? RegionName { get; set; }

    [JsonProperty("matchTimeFormatted")]
    public string? MatchTimeFormatted { get; set; }

    [JsonProperty("matchTimeOldFormatted")]
    public string? MatchTimeOldFormatted { get; set; }

    [JsonProperty("matchRoundFormatted")]
    public string? MatchRoundFormatted { get; set; }

    [JsonProperty("matchAwayTeamHallAndTime")]
    public string? MatchAwayTeamHallAndTime { get; set; }

    [JsonProperty("matchRowNbr")]
    public int MatchRowNbr { get; set; }

    [JsonProperty("matchOilPatternId")]
    public int MatchOilPatternId { get; set; }

    [JsonProperty("oilPatterns")]
    public object[]? OilPatterns { get; set; }

    [JsonProperty("matchOilPatternName")]
    public string? MatchOilPatternName { get; set; }

    [JsonProperty("matchFinishedHomeTeam")]
    public bool MatchFinishedHomeTeam { get; set; }

    [JsonProperty("matchFinishedAwayTeam")]
    public bool MatchFinishedAwayTeam { get; set; }

    [JsonProperty("matchReportStartDate")]
    public string? MatchReportStartDate { get; set; }

    [JsonProperty("matchReportEndDate")]
    public DateTime MatchReportEndDate { get; set; }

    [JsonProperty("cupId")]
    public int CupId { get; set; }

    [JsonProperty("cupRoundId")]
    public int CupRoundId { get; set; }

    [JsonProperty("cupCellNbr")]
    public int CupCellNbr { get; set; }

    [JsonProperty("cupNbrOfMatches")]
    public int CupNbrOfMatches { get; set; }

    [JsonProperty("lotTemplateIncludeMatchFromPreviousRound")]
    public bool LotTemplateIncludeMatchFromPreviousRound { get; set; }

    [JsonProperty("divisionSeasonStartMatchDayNo")]
    public int DivisionSeasonStartMatchDayNo { get; set; }
}
