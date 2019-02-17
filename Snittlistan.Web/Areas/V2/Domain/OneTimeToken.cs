namespace Snittlistan.Web.Areas.V2.Domain
{
    using System;
    using Raven.Abstractions;

    public class OneTimeToken
    {
        public OneTimeToken()
        {
            CreatedDate = SystemTime.UtcNow;
        }

        public string Id { get; set; }

        public string OneTimeKey { get; private set; }

        public DateTimeOffset? UsedDate { get; private set; }

        public DateTimeOffset CreatedDate { get; private set; }

        public string Payload { get; private set; }

        public void Activate(Action<string> action, string payload)
        {
            OneTimeKey = Guid.NewGuid().ToString();
            Payload = payload;
            action.Invoke(OneTimeKey);
        }

        public Result ApplyToken(Action action)
        {
            var span = SystemTime.UtcNow - CreatedDate;
            if (span.TotalDays > 1) return Result.Expired;

            action.Invoke();
            UsedDate = SystemTime.UtcNow;
            return Result.Ok;
        }

        public enum Result
        {
            Ok,
            Expired
        }
    }
}