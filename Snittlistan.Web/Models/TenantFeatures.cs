#nullable enable

namespace Snittlistan.Model;

public record TenantFeatures(
    bool RosterMailEnabled)
{
    public static string Key { get; } = "TenantFeatures";
};
