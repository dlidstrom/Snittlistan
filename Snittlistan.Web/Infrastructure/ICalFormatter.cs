using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Snittlistan.Web.Areas.V2.ViewModels;

namespace Snittlistan.Web.Infrastructure
{
    // ReSharper disable once InconsistentNaming
    public class ICalFormatter
        : BufferedMediaTypeFormatter
    {
        public ICalFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/iCal"));
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            if (type == typeof(RosterCalendarEvent))
            {
                return true;
            }

            var enumerableType = typeof(IEnumerable<RosterCalendarEvent>);
            return enumerableType.IsAssignableFrom(type);
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, mediaType);
            headers.ContentType = new MediaTypeHeaderValue("text/calendar")
            {
                CharSet = "utf-8"
            };
            headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "snittlistan.ics"
            };
        }

        public override void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
        {
            using (var writer = new StreamWriter(writeStream))
            {
                writer.WriteLine("BEGIN:VCALENDAR");
                writer.WriteLine("PRODID:-//Daniel Lidstrom AB//Snittlistan//EN");
                writer.WriteLine("VERSION:2.0");
                writer.WriteLine("X-PUBLISHED-TTL:PT1H");
                var rosters = value as IEnumerable<RosterCalendarEvent>;
                if (rosters != null)
                {
                    foreach (var roster in rosters)
                    {
                        WriteRoster(roster, writer);
                    }
                }
                else
                {
                    var roster = value as RosterCalendarEvent;
                    if (roster == null)
                    {
                        throw new InvalidOperationException("Cannot serialize type");
                    }

                    WriteRoster(roster, writer);
                }

                writer.WriteLine("END:VCALENDAR");
            }

            writeStream.Close();
        }

        private static void WriteRoster(RosterCalendarEvent roster, TextWriter writer)
        {
            writer.WriteLine("BEGIN:VEVENT");
            writer.WriteLine("UID:" + roster.Id);
            writer.WriteLine("DTSTAMP:" + string.Format("{0:yyyyMMddTHHmmssZ}", DateTime.UtcNow));
            writer.WriteLine("DTSTART:" + string.Format("{0:yyyyMMddTHHmmssZ}", roster.Date.ToUniversalTime()));
            writer.WriteLine("DTEND:" + string.Format("{0:yyyyMMddTHHmmssZ}", roster.Date.ToUniversalTime().AddMinutes(60 + 45)));
            writer.WriteLine("SUMMARY:{0} - {1}", roster.Team, roster.Opponent);
            var description = string.Format("DESCRIPTION:{0}", roster.Description);
            writer.WriteLine(description.Substring(0, Math.Min(description.Length, 74)));
            while (description.Length > 74)
            {
                description = description.Substring(74);
                writer.WriteLine(" " + description.Substring(0, Math.Min(description.Length, 74)));
            }

            writer.WriteLine("LOCATION:{0}", roster.Location);
            writer.WriteLine("END:VEVENT");
        }
    }
}