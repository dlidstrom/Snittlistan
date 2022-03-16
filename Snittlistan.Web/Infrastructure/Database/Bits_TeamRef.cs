#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snittlistan.Web.Infrastructure.Database;

public class Bits_TeamRef
{
    [Key]
    public int TeamRefId { get; private set; }

    public int ExternalTeamId { get; set; }

    public string TeamName { get; private set; } = string.Empty;

    public string TeamAlias { get; set; } = string.Empty;

    [InverseProperty(nameof(Bits_Match.HomeTeamRef))]
    public virtual ICollection<Bits_Match> HomeMatches { get; private set; } =
        new HashSet<Bits_Match>();

    [InverseProperty(nameof(Bits_Match.AwayTeamRef))]
    public virtual ICollection<Bits_Match> AwayMatches { get; private set; } =
        new HashSet<Bits_Match>();
}
