using System;
using System.Collections.Generic;

namespace Snittlistan.Web.Infrastructure.BackgroundTasks
{
    public class BackgroundTask
    {
        public BackgroundTask(object body)
        {
            if (body == null) throw new ArgumentNullException("body");
            Body = body;
            Exceptions = new List<Exception>();
            NextTry = DateTimeOffset.UtcNow;
        }

        public int Retries { get; private set; }

        public object Body { get; private set; }

        public bool IsFinished { get; private set; }

        public bool IsFailed { get; private set; }

        public DateTimeOffset NextTry { get; private set; }

        public List<Exception> Exceptions { get; private set; }

        public void MarkFinished()
        {
            IsFinished = true;
        }

        public void MarkFailed()
        {
            IsFailed = true;
        }

        public void UpdateNextTry(Exception exception)
        {
            Exceptions.Add(exception);

            Retries++;
            const int MaximumRetries = 5;
            if (Retries >= MaximumRetries)
            {
                MarkFailed();
            }
            else
            {
                NextTry = DateTimeOffset.UtcNow;
            }
        }
    }
}