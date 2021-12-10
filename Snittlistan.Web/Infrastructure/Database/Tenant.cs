#nullable enable

namespace Snittlistan.Web.Infrastructure.Database;
public class Tenant
{
    public Tenant(
        string hostname,
        string favicon,
        string appleTouchIcon,
        string appleTouchIconSize,
        string webAppTitle,
        int clubId,
        string teamFullName)
    {
        ClubId = clubId;
        TeamFullName = teamFullName;
        Hostname = hostname;
        Favicon = favicon;
        AppleTouchIcon = appleTouchIcon;
        AppleTouchIconSize = appleTouchIconSize;
        WebAppTitle = webAppTitle;
        CreatedDate = DateTime.Now;
    }

    private Tenant()
    {
    }

    public int TenantId { get; set; }

    public int ClubId { get; private set; }

    public string TeamFullName { get; private set; } = null!;

    public string Hostname { get; private set; } = null!;

    public string DatabaseName { get; private set; } = null!;

    public string Favicon { get; private set; } = null!;

    public string AppleTouchIcon { get; private set; } = null!;

    public string AppleTouchIconSize { get; private set; } = null!;

    public string WebAppTitle { get; private set; } = null!;

    public DateTime CreatedDate { get; private set; }

    public string Version { get; private set; } = null!;
}
