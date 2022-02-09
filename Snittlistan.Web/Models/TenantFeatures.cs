#nullable enable

namespace Snittlistan.Web.Infrastructure.Database;

public record TenantFeatures(
    bool RosterMailEnabled)
{
    public static string Key { get; } = "TenantFeatures";
};
