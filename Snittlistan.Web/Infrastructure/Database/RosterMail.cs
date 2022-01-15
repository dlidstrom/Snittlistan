#nullable enable

using System.ComponentModel.DataAnnotations.Schema;

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

    public int RosterMailId { get; private set; }

    public string RosterId { get; private set; } = null!;

    public DateTime? PublishedDate { get; private set; }

    public string Version { get; private set; } = null!;

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedDate { get; private set; }

    public void MarkPublished(DateTime when)
    {
        PublishedDate = when;
    }
}
