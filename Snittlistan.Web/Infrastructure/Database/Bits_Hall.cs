#nullable enable

using System.ComponentModel.DataAnnotations;

namespace Snittlistan.Web.Infrastructure.Database;

public class Bits_Hall
{
    [Key]
    public int HallId { get; set; }

    public int ExternalHallId { get; set; }

    public string HallName { get; set; } = null!;
}
