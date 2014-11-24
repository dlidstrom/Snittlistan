using System;
using System.Collections.Generic;
using Raven.Imports.Newtonsoft.Json;

namespace Snittlistan.Web.Infrastructure.BackgroundTasks
{
    public class BackgroundTask
    {
        [JsonConstructor]
        private BackgroundTask(object body)
        {
            if (body == null) throw new ArgumentNullException("body");
            Body = body;
            Exceptions = new List<Exception>();
            NextTry = DateTimeOffset.UtcNow;
        }

        public string Id { get; private set; }

        public int Retries { get; private set; }

        public object Body { get; private set; }

        public bool IsFinished { get; private set; }

        public bool IsFailed { get; private set; }

        public DateTimeOffset NextTry { get; private set; }

        public List<Exception> Exceptions { get; private set; }

        public static BackgroundTask Create<TBody>(TBody body) where TBody : class
        {
            return new BackgroundTask(body);
        }

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

        public string GetInfo()
        {
            var info = string.Format("Type: {0}{1}{2}", Body.GetType(), Environment.NewLine, Body);
            return info;
        }
    }
}