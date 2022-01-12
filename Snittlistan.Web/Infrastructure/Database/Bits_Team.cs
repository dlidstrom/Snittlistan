#nullable enable

namespace Snittlistan.Web.Infrastructure.Database;

public class Bits_Team
{
    public int ExternalTeamId { get; set; }

    public string TeamName { get; set; } = null!;

    public string TeamAlias { get; set; } = null!;
}
