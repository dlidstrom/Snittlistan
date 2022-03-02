#nullable enable

namespace Snittlistan.Model;

public record UserSettings(
    bool RosterMailEnabled,
    bool AbsenceMailEnabled,
    bool MatchResultMailEnabled)
{
    public static UserSettings Default = new(
        true,
        true,
        true);

    public static string GetKey(string playerId)
    {
        return $"settings:{playerId}";
    }
}
