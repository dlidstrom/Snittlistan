#nullable enable

namespace Snittlistan.Web.Infrastructure.Database
{
    using System;

    public class DelayedTask
    {
        public DelayedTask(string businessKey, string data, DateTime publishDate)
        {
            BusinessKey = businessKey;
            Data = data;
            PublishDate = publishDate;
            CreatedDate = DateTime.Now;
        }

        private DelayedTask()
        {
        }

        public int DelayedTaskId { get; private set; }

        public string BusinessKey { get; private set; } = null!;

        public string Data { get; private set; } = null!;

        public DateTime PublishDate { get; private set; }

        public DateTime? PublishedDate { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public void MarkAsPublished(DateTime timestamp)
        {
            PublishedDate = timestamp;
        }
    }
}
