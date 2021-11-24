#nullable enable

namespace Snittlistan.Web.Infrastructure.Database
{
    using System.Data.Entity;

    public interface IBitsContext
    {
        public IDbSet<Bits_Team> Teams { get; }

        public IDbSet<Bits_Hall> Hallar { get; }
    }
}
