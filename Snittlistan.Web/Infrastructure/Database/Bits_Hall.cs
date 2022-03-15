#nullable enable

using System.ComponentModel.DataAnnotations;

namespace Snittlistan.Web.Infrastructure.Database;

public class Bits_Hall
{
    [Key]
    public int HallId { get; private set; }

    public int ExternalHallId { get; private set; }

    public string HallName { get; private set; } = string.Empty;

    public int HallRefId { get; set; }

    public Bits_HallRef HallRef { get; private set; } = null!;
}
