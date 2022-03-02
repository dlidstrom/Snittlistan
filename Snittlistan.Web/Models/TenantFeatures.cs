#nullable enable

namespace Snittlistan.Web.Models;

public record TenantFeatures(
    bool RosterMailEnabled)
{
    public static string Key { get; } = "TenantFeatures";
};
