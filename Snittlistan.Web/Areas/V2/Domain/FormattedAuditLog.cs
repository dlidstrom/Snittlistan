#nullable enable

namespace Snittlistan.Web.Areas.V2.Domain;

public class FormattedAuditLog
{
    public static FormattedAuditLog Empty = new(string.Empty, new FormattedAuditLogEntry[0]);

    public FormattedAuditLog(string title, FormattedAuditLogEntry[] entries)
    {
        Title = title;
        Entries = entries;
    }

    public string Title { get; }

    public FormattedAuditLogEntry[] Entries { get; }
}
