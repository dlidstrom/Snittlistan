#nullable enable

namespace Snittlistan.Web.Infrastructure.Database;

public record UserSettings(
    bool RosterMailEnabled)
{
    public static UserSettings Default = new(true);

    public static string GetKey(string playerId)
    {
        return $"settings:{playerId}";
    }
}
