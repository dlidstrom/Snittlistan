#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snittlistan.Web.Infrastructure.Database;

/*
 * SELECT
    m.external_match_id,
    tr_home.team_name AS home_team_name,
    tr_home.team_alias AS home_team_alias,
    tr_away.team_name AS away_team_name,
    tr_away.team_alias AS away_team_alias,
    op.oil_profile_name,
    hr.hall_name
FROM
    bits.match m
    JOIN bits.team_ref tr_home ON m.match_home_team_ref_id = tr_home.team_ref_id
    JOIN bits.team_ref tr_away ON m.match_away_team_ref_id = tr_away.team_ref_id
    JOIN bits.oil_profile op ON m.match_oil_profile_id = op.oil_profile_id
    JOIN bits.hall_ref hr ON m.match_hall_ref_id = hr.hall_ref_id
WHERE
    m.external_match_id = 3235829;

 */
public class Bits_Match
{
    [Key]
    public int MatchId { get; set; }

    public int ExternalMatchId { get; set; }

    [Column("match_home_team_ref_id")]
    public int HomeTeamRefId { get; set; }

    public virtual Bits_TeamRef HomeTeamRef { get; set; } = null!;

    [Column("match_away_team_ref_id")]
    public int AwayTeamRefId { get; set; }

    public virtual Bits_TeamRef AwayTeamRef { get; set; } = null!;

    [Column("match_oil_profile_id")]
    public int OilProfileId { get; set; }

    public virtual Bits_OilProfile OilProfile { get; set; } = null!;
}
public class Bits_TeamRef
{
    [Key]
    public int TeamRefId { get; set; }

    public virtual ICollection<Bits_Match> Matches { get; set; } = null!;
}
public class Bits_OilProfile
{
    [Key]
    public int OilProfileId { get; set; }
}
public class Bits_HallRef
{
    [Key]
    public int HallRefId { get; set; }
}
