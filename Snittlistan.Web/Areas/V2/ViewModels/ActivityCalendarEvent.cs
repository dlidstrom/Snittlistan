#nullable enable

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    using System;
    using System.IO;
    using Domain;
    using Infrastructure;

    public class ActivityCalendarEvent : CalendarEvent
    {
        private readonly string id;
        private readonly DateTime date;
        private readonly string summary;
        private readonly string description;

        public ActivityCalendarEvent(Activity activity)
        {
            id = activity.Id!;
            date = activity.Date;
            summary = activity.Title;
            description = activity.Message;
        }

        public override void Write(TextWriter writer)
        {
            writer.WriteLine("BEGIN:VEVENT");
            writer.WriteLine("UID:" + id);
            writer.WriteLine("DTSTAMP:" + $"{DateTime.UtcNow:yyyyMMddTHHmmssZ}");
            writer.WriteLine("DTSTART:" + $"{date.ToUniversalTime():yyyyMMddTHHmmssZ}");
            writer.WriteLine("DTEND:" + $"{date.ToUniversalTime():yyyyMMddTHHmmssZ}");
            writer.WriteLine($"SUMMARY:{summary}");
            string desc = $"DESCRIPTION:{description}";
            WriteWithLineFeeds(writer, desc);

            //writer.WriteLine("LOCATION:{0}", location);
            writer.WriteLine("END:VEVENT");
        }
    }
}
