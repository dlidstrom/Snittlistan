#nullable enable

namespace Snittlistan.Web.Areas.V2.Domain
{
    using System;
    using Raven.Abstractions;

    public class AuditLogEntry
    {
        public AuditLogEntry(
            string userId,
            string action,
            Guid correlationId,
            object change,
            object before,
            object after,
            DateTime? date = null)
        {
            UserId = userId;
            Action = action;
            CorrelationId = correlationId;
            Date = date ?? SystemTime.UtcNow.ToLocalTime();
            Change = change;
            Before = before;
            After = after;
        }

        public string UserId { get; }
        public string Action { get; }
        public Guid CorrelationId { get; }
        public DateTime? Date { get; }
        public object Change { get; }
        public object Before { get; private set; }
        public object After { get; private set; }

        public void SetBefore(object before)
        {
            Before = before;
        }

        public void SetAfter(object after)
        {
            After = after;
        }

        public class PropertyChange<TPropertyType>
        {
            public PropertyChange(TPropertyType oldValue, TPropertyType newValue)
            {
                OldValue = oldValue;
                NewValue = newValue;
            }

            public TPropertyType OldValue { get; }
            public TPropertyType NewValue { get; }
        }

        public static class PropertyChange
        {
            public static PropertyChange<TPropertyType> Create<TPropertyType>(
                TPropertyType oldValue,
                TPropertyType newValue)
            {
                return new PropertyChange<TPropertyType>(oldValue, newValue);
            }
        }
    }
}
