using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

#nullable enable

namespace Snittlistan.Web.Infrastructure;
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
        if (type == typeof(CalendarEvent))
        {
            return true;
        }

        Type enumerableType = typeof(IEnumerable<CalendarEvent>);
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
        using (StreamWriter writer = new(writeStream))
        {
            writer.WriteLine("BEGIN:VCALENDAR");
            writer.WriteLine("PRODID:-//Daniel Lidstrom AB//Snittlistan//EN");
            writer.WriteLine("VERSION:2.0");
            writer.WriteLine("X-PUBLISHED-TTL:PT1H");
            writer.WriteLine("X-WR-CALNAME:Snittlistan");
            if (value is IEnumerable<CalendarEvent> calendarEvents)
            {
                foreach (CalendarEvent calendarEvent in calendarEvents)
                {
                    calendarEvent.Write(writer);
                }
            }
            else
            {
                if (value is not CalendarEvent calendarEvent)
                {
                    throw new InvalidOperationException("Cannot serialize type");
                }

                calendarEvent.Write(writer);
            }

            writer.WriteLine("END:VCALENDAR");
        }

        writeStream.Close();
    }
}
