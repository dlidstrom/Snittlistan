#nullable enable

namespace Snittlistan.Web.Infrastructure.Database;

public interface IBitsContext : IDisposable
{
    public IDbSet<Bits_Team> Team { get; }

    public IDbSet<Bits_HallRef> HallRef { get; }

    public IDbSet<Bits_Hall> Hall { get; }

    public IDbSet<Bits_Match> Match { get; }

    public IDbSet<Bits_TeamRef> TeamRef { get; }

    public IDbSet<Bits_OilProfile> OilProfile { get; }

    public IDbSet<Bits_VMatchHeadInfo> VMatchHeadInfo { get; }
}
