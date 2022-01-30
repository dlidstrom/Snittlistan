#nullable enable

namespace Snittlistan.Web.Infrastructure.Database;

public class RosterMail : HasVersion
{
    public RosterMail(string rosterKey)
    {
        RosterKey = rosterKey;
    }

    private RosterMail()
    {
    }

    public int RosterMailId { get; private set; }

    public string RosterKey { get; private set; } = null!;

    public DateTime? PublishedDate { get; private set; }

    public void MarkPublished(DateTime when)
    {
        PublishedDate = when;
    }
}
