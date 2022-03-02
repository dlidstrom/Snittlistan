#nullable enable

namespace Snittlistan.Models;

public record TenantFeatures(
    bool RosterMailEnabled)
{
    public static string Key { get; } = "TenantFeatures";
};
