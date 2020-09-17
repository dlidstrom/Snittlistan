namespace Snittlistan.Web.Areas.V2.Domain
{
    public class AuditLogEntry
    {
        public AuditLogEntry(string userId, string action, object change, object before, object after)
        {
            UserId = userId;
            Action = action;
            Change = change;
            Before = before;
            After = after;
        }

        public string UserId { get; }
        public string Action { get; }
        public object Change { get; }
        public object Before { get; }
        public object After { get; }

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
