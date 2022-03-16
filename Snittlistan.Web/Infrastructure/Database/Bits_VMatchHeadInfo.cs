#nullable enable

using System.ComponentModel.DataAnnotations;

namespace Snittlistan.Web.Infrastructure.Database;

public class Bits_VMatchHeadInfo
{
    [Key]
    public int ExternalMatchId { get; private set; }

    public DateTime MatchDateTime { get; private set; }

    public string HomeTeamName { get; private set; } = string.Empty;

    public string HomeTeamAlias { get; private set; } = string.Empty;

    public string AwayTeamName { get; private set; } = string.Empty;

    public string AwayTeamAlias { get; private set; } = string.Empty;

    public int OilProfileId { get; private set; }

    public string OilProfileName { get; private set; } = string.Empty;

    public string HallName { get; private set; } = string.Empty;
}
