#nullable enable

namespace Snittlistan.Web.Infrastructure.Database;

public class RosterMail
{
    public RosterMail(string rosterId)
    {
        RosterId = rosterId;
    }

    private RosterMail()
    {
    }

    public string RosterId { get; private set; } = null!;

    public string Version { get; private set; } = null!;
}
