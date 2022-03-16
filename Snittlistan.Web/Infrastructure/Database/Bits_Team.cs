#nullable enable

using System.ComponentModel.DataAnnotations;

namespace Snittlistan.Web.Infrastructure.Database;

public class Bits_Team
{
    [Key]
    public int TeamId { get; set; }

    public int ExternalTeamId { get; set; }

    public string TeamName { get; set; } = null!;

    public string TeamAlias { get; set; } = null!;
}
