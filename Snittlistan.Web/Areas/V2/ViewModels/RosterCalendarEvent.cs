using System.IO;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.ReadModels;

#nullable enable

namespace Snittlistan.Web.Areas.V2.ViewModels;
public class RosterCalendarEvent : CalendarEvent
{
    private readonly string id;
    private readonly DateTime date;
    private readonly string team;
    private readonly string opponent;
    private readonly string location;
    private readonly string description;

    public RosterCalendarEvent(Roster roster, ResultHeaderReadModel resultHeaderReadModel)
    {
        id = roster.Id!;
        date = roster.Date;
        team = roster.Team;
        opponent = roster.Opponent!;
        location = roster.Location!;
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
        string desc = $"DESCRIPTION:{description}";
        WriteWithLineFeeds(writer, desc);

        writer.WriteLine("LOCATION:{0}", location);
        writer.WriteLine("END:VEVENT");
    }
}
