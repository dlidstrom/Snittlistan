using System;

namespace Snittlistan.Web.Areas.V2.Domain
{
    public class FormattedAuditLogEntry
    {
        public FormattedAuditLogEntry(string userId, string action, DateTime? date)
        {
            UserId = userId;
            Action = action;
            Date = date;
        }

        public string UserId { get; }
        public string Action { get; }
        public DateTime? Date { get; }
    }
}
