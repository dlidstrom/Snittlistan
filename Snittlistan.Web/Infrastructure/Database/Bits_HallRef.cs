#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snittlistan.Web.Infrastructure.Database;

public class Bits_HallRef
{
    [Key]
    public int HallRefId { get; private set; }

    public int ExternalHallId { get; private set; }

    //[ForeignKey(nameof(Hall))]
    //public int HallId { get; private set; }

    public string HallName { get; private set; } = string.Empty;

    public Bits_Hall? Hall { get; private set; }
}
