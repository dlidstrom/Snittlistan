namespace Snittlistan.Web.Areas.V2.ViewModels
{
    using System;
    using System.IO;
    using Snittlistan.Web.Areas.V2.Domain;
    using Snittlistan.Web.Areas.V2.ReadModels;

    public abstract class CalendarEvent
    {
        protected const string TextLineFeed = @"\" + "n";

        public abstract void Write(TextWriter writer);
    }

    public class RosterCalendarEvent : CalendarEvent
    {
        private readonly string id;
        private DateTime date;
        private readonly string team;
        private readonly string opponent;
        private readonly string location;
        private readonly string description;

        public RosterCalendarEvent(Roster roster, ResultHeaderReadModel resultHeaderReadModel)
        {
            id = roster.Id;
            date = roster.Date;
            team = roster.Team;
            opponent = roster.Opponent;
            location = roster.Location;
            description = string.Empty;
            if (resultHeaderReadModel != null)
            {
                description = resultHeaderReadModel.MatchCommentary
                              + TextLineFeed
                              + TextLineFeed
                              + string.Join(TextLineFeed + TextLineFeed, resultHeaderReadModel.BodyText);
            }
        }

        public override void Write(TextWriter writer)
        {
            writer.WriteLine("BEGIN:VEVENT");
            writer.WriteLine("UID:" + id);
            writer.WriteLine("DTSTAMP:" + $"{DateTime.UtcNow:yyyyMMddTHHmmssZ}");
            writer.WriteLine("DTSTART:" + $"{date.ToUniversalTime():yyyyMMddTHHmmssZ}");
            writer.WriteLine("DTEND:" + $"{date.ToUniversalTime().AddMinutes(60 + 40):yyyyMMddTHHmmssZ}");
            writer.WriteLine("SUMMARY:{0} - {1}", team, opponent);
            var desc = $"DESCRIPTION:{description}";
            writer.WriteLine(desc.Substring(0, Math.Min(desc.Length, 74)));
            while (desc.Length > 74)
            {
                desc = desc.Substring(74);
                writer.WriteLine(" " + desc.Substring(0, Math.Min(desc.Length, 74)));
            }

            writer.WriteLine("LOCATION:{0}", location);
            writer.WriteLine("END:VEVENT");
        }
    }

    public class ActivityCalendarEvent : CalendarEvent
    {
        private readonly string id;
        private DateTime date;
        private readonly string summary;
        private readonly string description;

        public ActivityCalendarEvent(Activity activity)
        {
            id = activity.Id;
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
            var desc = $"DESCRIPTION:{description}";
            writer.WriteLine(desc.Substring(0, Math.Min(desc.Length, 74)));
            while (desc.Length > 74)
            {
                desc = desc.Substring(74);
                writer.WriteLine(" " + desc.Substring(0, Math.Min(desc.Length, 74)));
            }

            //writer.WriteLine("LOCATION:{0}", location);
            writer.WriteLine("END:VEVENT");
        }
    }
}