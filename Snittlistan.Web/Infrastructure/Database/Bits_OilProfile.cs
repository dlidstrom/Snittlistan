#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snittlistan.Web.Infrastructure.Database;

public class Bits_OilProfile
{
    [Key]
    public int OilProfileId { get; set; }

    [InverseProperty(nameof(Bits_Match.OilProfile))]
    public virtual ICollection<Bits_Match> Matches { get; set; } =
        new HashSet<Bits_Match>();
}
