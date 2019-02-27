namespace Snittlistan.Web.Areas.V2.Domain
{
    using System;
    using Raven.Imports.Newtonsoft.Json;

    public class Activity
    {
        [JsonConstructor]
        private Activity(int season, string title, DateTime date, string message)
        {
            Season = season;
            Title = title;
            Date = date;
            Message = message;
        }

        public string Id { get; set; }

        public int Season { get; private set; }

        public string Title { get; private set; }

        public DateTime Date { get; private set; }

        public string Message { get; private set; }

        public void Update(int season, string title, DateTime date, string message)
        {
            if (message.Length > 1024) throw new ArgumentOutOfRangeException(nameof(message), "Max 1024");
            Season = season;
            Title = title;
            Date = date;
            Message = message;
        }

        public static Activity Create(
            int season,
            string title,
            DateTime date,
            string message)
        {
            if (message.Length > 1024) throw new ArgumentOutOfRangeException(nameof(message), "Max 1024");
            return new Activity(season, title, date, message);
        }
    }
}