#nullable enable

namespace Snittlistan.Web.Areas.V2.Controllers.Api
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Infrastructure;
    using Raven.Abstractions;
    using Snittlistan.Web.Areas.V2.Domain;
    using Snittlistan.Web.Areas.V2.Indexes;
    using Snittlistan.Web.Areas.V2.ReadModels;
    using Snittlistan.Web.Areas.V2.ViewModels;
    using Snittlistan.Web.Controllers;
    using Snittlistan.Web.Helpers;

    public class CalendarController : AbstractApiController
    {
        public HttpResponseMessage Get()
        {
            int season = DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);

            // rosters
            Roster[] rosters = DocumentSession.Query<Roster, RosterSearchTerms>()
                                         .Where(r => r.Season == season)
                                         .ToArray();
            IEnumerable<string> resultIds = rosters.Select(x => ResultHeaderReadModel.IdFromBitsMatchId(x.BitsMatchId, x.Id!));
            ResultHeaderReadModel[] results = DocumentSession.Load<ResultHeaderReadModel>(resultIds);
            Dictionary<string, ResultHeaderReadModel> resultsDictionary = results.Where(x => x != null)
                                           .ToDictionary(x => x.Id!);
            List<CalendarEvent> calendarEvents = new();
            foreach (Roster roster in rosters)
            {
                _ = resultsDictionary.TryGetValue(ResultHeaderReadModel.IdFromBitsMatchId(roster.BitsMatchId, roster.Id!), out ResultHeaderReadModel resultHeaderReadModel);
                RosterCalendarEvent rosterCalendarEvent = new(roster, resultHeaderReadModel);
                calendarEvents.Add(rosterCalendarEvent);
            }

            // activities
            Activity[] activities = DocumentSession.Query<Activity, ActivityIndex>()
                                            .Where(x => x.Season == season)
                                            .ToArray();

            foreach (Activity activity in activities)
            {
                ActivityCalendarEvent activityCalendarEvent = new(activity);
                calendarEvents.Add(activityCalendarEvent);
            }

            return Request.CreateResponse(HttpStatusCode.OK, calendarEvents.ToArray(), new MediaTypeHeaderValue("text/iCal"));
        }
    }
}
