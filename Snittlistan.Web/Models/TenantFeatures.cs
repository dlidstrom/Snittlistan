#nullable enable

namespace Snittlistan.Web.Models;

public record TenantFeatures(
    bool RosterMailEnabled,
    int RosterMailDelayMinutes)
{
    public static string Key { get; } = "TenantFeatures";

    public static TenantFeatures Default = new(
        true,
        10);
};
