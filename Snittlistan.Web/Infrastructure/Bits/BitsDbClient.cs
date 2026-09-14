#nullable enable

using System.Data.Entity;
using Newtonsoft.Json;
using Npgsql;
using Snittlistan.Web.Infrastructure.Bits.Contracts;
using Snittlistan.Web.Infrastructure.Database;

namespace Snittlistan.Web.Infrastructure.Bits;

/// <summary>
/// Reads BITS match/roster data directly from the sportbowling-prod Postgres database
/// (the "bits" schema), which is kept in sync with the BITS API, instead of calling the
/// external BITS REST API over HTTP.
/// </summary>
public class BitsDbClient : IBitsClient
{
    private const string HeadInfoSql = @"
        select m.external_match_id as ExternalMatchId,
               m.match_date_time as MatchDateTime,
               m.match_round_id as MatchRoundId,
               -- match_finished reflects BITS' own status, not whether results have
               -- landed in this replica; results_received_date is what callers actually
               -- need before they can parse/register a result.
               (m.results_received_date is not null) as MatchFinished,
               hr.hall_name as HallName,
               op.oil_profile_name as OilProfileName,
               op.external_oil_profile_id as ExternalOilProfileId,
               th.team_alias as HomeAlias,
               ta.team_alias as AwayAlias,
               ch.external_club_id as HomeClubId,
               ca.external_club_id as AwayClubId,
               s.external_season_id as ExternalSeasonId
        from bits.match m
        join bits.hall_ref hr on hr.hall_ref_id = m.match_hall_ref_id
        join bits.oil_profile op on op.oil_profile_id = m.match_oil_profile_id
        join bits.team_ref th on th.team_ref_id = m.match_home_team_ref_id
        join bits.team_ref ta on ta.team_ref_id = m.match_away_team_ref_id
        join bits.team tth on tth.team_id = th.team_id
        join bits.team tta on tta.team_id = ta.team_id
        join bits.club_ref crh on crh.club_ref_id = tth.club_ref_id
        join bits.club_ref cra on cra.club_ref_id = tta.club_ref_id
        join bits.club ch on ch.club_id = crh.club_id
        join bits.club ca on ca.club_id = cra.club_id
        join bits.division_season ds on ds.division_season_id = m.match_division_season_id
        join bits.season s on s.season_id = ds.season_id
        where m.external_match_id = @matchId";

    private const string TeamSql = @"
        select distinct t.external_team_id as TeamId,
               t.team_name as TeamName,
               t.team_alias as TeamAlias
        from bits.team t
        join bits.club_ref cr on cr.club_ref_id = t.club_ref_id
        join bits.club c on c.club_id = cr.club_id
        join bits.team_ref tr on tr.team_id = t.team_id
        join bits.match m on m.match_home_team_ref_id = tr.team_ref_id or m.match_away_team_ref_id = tr.team_ref_id
        join bits.division_season ds on ds.division_season_id = m.match_division_season_id
        join bits.season s on s.season_id = ds.season_id
        where c.external_club_id = @clubId and s.external_season_id = @seasonId";

    private const string DivisionSql = @"
        select distinct d.external_division_id as DivisionId,
               d.division_name as DivisionName
        from bits.division d
        join bits.division_season ds on ds.division_id = d.division_id
        join bits.season s on s.season_id = ds.season_id
        join bits.match m on m.match_division_season_id = ds.division_season_id
        join bits.team_ref tr on tr.team_ref_id = m.match_home_team_ref_id or tr.team_ref_id = m.match_away_team_ref_id
        join bits.team t on t.team_id = tr.team_id
        where t.external_team_id = @teamId and s.external_season_id = @seasonId";

    private const string MatchRoundSql = @"
        select m.external_match_id as MatchId,
               m.match_date_time as MatchDateTime,
               m.match_round_id as MatchRoundId,
               m.match_nbr_of_players as MatchNbrOfPlayers,
               ms.external_match_status_id as MatchStatus,
               hr.hall_name as HallName,
               op.oil_profile_name as OilProfileName,
               op.external_oil_profile_id as ExternalOilProfileId,
               th.team_alias as HomeAlias,
               ta.team_alias as AwayAlias,
               ch.external_club_id as HomeClubId,
               ca.external_club_id as AwayClubId,
               s.external_season_id as ExternalSeasonId
        from bits.match m
        join bits.hall_ref hr on hr.hall_ref_id = m.match_hall_ref_id
        join bits.oil_profile op on op.oil_profile_id = m.match_oil_profile_id
        join bits.match_status ms on ms.match_status_id = m.match_status_id
        join bits.team_ref th on th.team_ref_id = m.match_home_team_ref_id
        join bits.team_ref ta on ta.team_ref_id = m.match_away_team_ref_id
        join bits.team tth on tth.team_id = th.team_id
        join bits.team tta on tta.team_id = ta.team_id
        join bits.club_ref crh on crh.club_ref_id = tth.club_ref_id
        join bits.club_ref cra on cra.club_ref_id = tta.club_ref_id
        join bits.club ch on ch.club_id = crh.club_id
        join bits.club ca on ca.club_id = cra.club_id
        join bits.division_season ds on ds.division_season_id = m.match_division_season_id
        join bits.division d on d.division_id = ds.division_id
        join bits.season s on s.season_id = ds.season_id
        where d.external_division_id = @divisionId
          and s.external_season_id = @seasonId
          and (tth.external_team_id = @teamId or tta.external_team_id = @teamId)";

    public Task<HeadInfo> GetHeadInfo(int matchId)
    {
        using BitsContext context = new();
        HeadInfoRow row = context.Database
            .SqlQuery<HeadInfoRow>(HeadInfoSql, new NpgsqlParameter("matchId", matchId))
            .SingleOrDefault()
            ?? throw new Exception($"BITS match not found: {matchId}");

        HeadInfo result = new()
        {
            MatchId = row.ExternalMatchId,
            MatchHomeClubId = row.HomeClubId,
            MatchAwayClubId = row.AwayClubId,
            MatchHomeTeamAlias = row.HomeAlias,
            MatchAwayTeamAlias = row.AwayAlias,
            MatchRoundId = row.MatchRoundId,
            MatchDate = row.MatchDateTime.ToString("yyyy-MM-dd"),
            MatchTime = (row.MatchDateTime.Hour * 100) + row.MatchDateTime.Minute,
            MatchHallName = row.HallName,
            MatchOilPatternName = row.OilProfileName,
            MatchOilPatternId = row.ExternalOilProfileId,
            MatchFinished = row.MatchFinished,
            MatchSeason = row.ExternalSeasonId,
        };
        return Task.FromResult(result);
    }

    public Task<HeadResultInfo> GetHeadResultInfo(int matchId)
    {
        return Task.FromResult(GetJson<HeadResultInfo>(
            matchId,
            "bits.head_result_info",
            "bits_head_result_info"));
    }

    public Task<MatchResults> GetMatchResults(int matchId)
    {
        return Task.FromResult(GetJson<MatchResults>(
            matchId,
            "bits.match_results",
            "bits_match_results"));
    }

    public Task<MatchScores> GetMatchScores(int matchId)
    {
        return Task.FromResult(GetJson<MatchScores>(
            matchId,
            "bits.match_scores",
            "bits_match_scores"));
    }

    public Task<TeamResult[]> GetTeam(int clubId, int seasonId)
    {
        using BitsContext context = new();
        TeamResult[] result = context.Database
            .SqlQuery<TeamRow>(
                TeamSql,
                new NpgsqlParameter("clubId", clubId),
                new NpgsqlParameter("seasonId", seasonId))
            .Select(x => new TeamResult
            {
                TeamId = x.TeamId,
                TeamName = x.TeamName,
                TeamAlias = x.TeamAlias
            })
            .ToArray();
        return Task.FromResult(result);
    }

    public Task<DivisionResult[]> GetDivisions(int teamId, int seasonId)
    {
        using BitsContext context = new();
        DivisionResult[] result = context.Database
            .SqlQuery<DivisionRow>(
                DivisionSql,
                new NpgsqlParameter("teamId", teamId),
                new NpgsqlParameter("seasonId", seasonId))
            .Select(x => new DivisionResult
            {
                DivisionId = x.DivisionId,
                DivisionName = x.DivisionName
            })
            .ToArray();
        return Task.FromResult(result);
    }

    public Task<MatchRound[]> GetMatchRounds(int teamId, int divisionId, int seasonId)
    {
        using BitsContext context = new();
        MatchRound[] result = context.Database
            .SqlQuery<MatchRoundRow>(
                MatchRoundSql,
                new NpgsqlParameter("teamId", teamId),
                new NpgsqlParameter("divisionId", divisionId),
                new NpgsqlParameter("seasonId", seasonId))
            .Select(x => new MatchRound
            {
                MatchId = x.MatchId,
                MatchDate = x.MatchDateTime.ToString("yyyy-MM-dd"),
                MatchTime = (x.MatchDateTime.Hour * 100) + x.MatchDateTime.Minute,
                MatchRoundId = x.MatchRoundId,
                MatchStatus = x.MatchStatus,
                MatchHallName = x.HallName,
                MatchOilPatternName = x.OilProfileName,
                MatchOilPatternId = x.ExternalOilProfileId,
                MatchHomeTeamAlias = x.HomeAlias,
                MatchAwayTeamAlias = x.AwayAlias,
                HomeTeamClubId = x.HomeClubId,
                AwayTeamClubId = x.AwayClubId,
                MatchSeason = x.ExternalSeasonId,
                MatchNbrOfPlayers = x.MatchNbrOfPlayers
            })
            .ToArray();
        return Task.FromResult(result);
    }

    private static TResult GetJson<TResult>(int matchId, string table, string jsonColumn)
        where TResult : class
    {
        using BitsContext context = new();
        string sql = $@"
            select t.{jsonColumn}::text as Json
            from {table} t
            join bits.match m on m.match_id = t.match_id
            where m.external_match_id = @matchId";
        string? json = context.Database
            .SqlQuery<string>(sql, new NpgsqlParameter("matchId", matchId))
            .SingleOrDefault();
        if (json == null)
        {
            throw new Exception($"BITS {table} not found for match: {matchId}");
        }

        return JsonConvert.DeserializeObject<TResult>(json)
            ?? throw new Exception($"Failed to deserialize {table} for match: {matchId}");
    }

    private class HeadInfoRow
    {
        public int ExternalMatchId { get; set; }
        public DateTime MatchDateTime { get; set; }
        public int MatchRoundId { get; set; }
        public bool MatchFinished { get; set; }
        public string HallName { get; set; } = string.Empty;
        public string OilProfileName { get; set; } = string.Empty;
        public int ExternalOilProfileId { get; set; }
        public string HomeAlias { get; set; } = string.Empty;
        public string AwayAlias { get; set; } = string.Empty;
        public int HomeClubId { get; set; }
        public int AwayClubId { get; set; }
        public int ExternalSeasonId { get; set; }
    }

    private class TeamRow
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public string TeamAlias { get; set; } = string.Empty;
    }

    private class DivisionRow
    {
        public int DivisionId { get; set; }
        public string DivisionName { get; set; } = string.Empty;
    }

    private class MatchRoundRow
    {
        public int MatchId { get; set; }
        public DateTime MatchDateTime { get; set; }
        public int MatchRoundId { get; set; }
        public int MatchNbrOfPlayers { get; set; }
        public int MatchStatus { get; set; }
        public string HallName { get; set; } = string.Empty;
        public string OilProfileName { get; set; } = string.Empty;
        public int ExternalOilProfileId { get; set; }
        public string HomeAlias { get; set; } = string.Empty;
        public string AwayAlias { get; set; } = string.Empty;
        public int HomeClubId { get; set; }
        public int AwayClubId { get; set; }
        public int ExternalSeasonId { get; set; }
    }
}
