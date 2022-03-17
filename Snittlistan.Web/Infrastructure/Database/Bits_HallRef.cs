#nullable enable

using System.ComponentModel.DataAnnotations;

namespace Snittlistan.Web.Infrastructure.Database;

public class Bits_HallRef
{
    [Key]
    public int HallRefId { get; private set; }

    public int ExternalHallId { get; private set; }

    public int? HallId { get; private set; }

    public string HallName { get; private set; } = string.Empty;
}
