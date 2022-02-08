#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snittlistan.Web.Infrastructure.Database;

public abstract class HasVersion
{
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [ConcurrencyCheck]
    [Column("xmin")]
    public string Version { get; private set; } = null!;

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedDate { get; private set; }
}
