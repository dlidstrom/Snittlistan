namespace Snittlistan.Web.Infrastructure.Database
{
    using System;

    public class DelayedTask
    {
        public int DelayedTaskId { get; private set; }

        public string BusinessKey { get; private set; }

        public string Data { get; private set; }

        public DateTime PublishDate { get; private set; }

        public DateTime CreatedDate { get; private set; }
    }
}
