#nullable enable

namespace EventStoreLite
{
    using System.Collections.Generic;

    public class EventStream
    {
        public EventStream()
        {
            History = new List<IDomainEvent>();
        }

        public string? Id { get; set; }

        public List<IDomainEvent> History { get; internal set; }
    }
}
