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

        public void Activate(Action<string> action)
        {
            OneTimeKey = Guid.NewGuid().ToString();
            action.Invoke(OneTimeKey);
        }

        public void ApplyToken(Action action)
        {
            if (UsedDate.HasValue) throw new Exception("Token has been used");
            var span = SystemTime.UtcNow - CreatedDate;
            if (span.TotalDays > 1) throw new Exception("Token has expired");

            action.Invoke();
            UsedDate = SystemTime.UtcNow;
        }
    }
}