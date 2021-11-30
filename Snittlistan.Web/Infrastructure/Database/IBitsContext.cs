
using System.Data.Entity;

#nullable enable

namespace Snittlistan.Web.Infrastructure.Database;
public interface IBitsContext
{
    public IDbSet<Bits_Team> Teams { get; }

    public IDbSet<Bits_Hall> Hallar { get; }
}
