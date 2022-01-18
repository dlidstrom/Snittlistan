#nullable enable

namespace Snittlistan.Web.Infrastructure.Database;

public class RosterMail : HasVersion
{
    public RosterMail(string rosterId)
    {
        RosterId = rosterId;
    }

    private RosterMail()
    {
    }

    public int RosterMailId { get; private set; }

    public string RosterId { get; private set; } = null!;

    public DateTime? PublishedDate { get; private set; }

    public void MarkPublished(DateTime when)
    {
        PublishedDate = when;
    }
}
