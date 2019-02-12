namespace Snittlistan.Web.Areas.V2.Controllers.Api
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
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
            var season = DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);

            var rosters = DocumentSession.Query<Roster, RosterSearchTerms>()
                                         .Where(r => r.Season == season)
                                         .ToArray();
            var resultIds = rosters.Select(x => ResultHeaderReadModel.IdFromBitsMatchId(x.BitsMatchId, x.Id));
            var results = DocumentSession.Load<ResultHeaderReadModel>(resultIds);
            var resultsDictionary = results.Where(x => x != null)
                                           .ToDictionary(x => x.Id);
            var calendarEvents = new List<RosterCalendarEvent>();
            foreach (var roster in rosters)
            {
                resultsDictionary.TryGetValue(ResultHeaderReadModel.IdFromBitsMatchId(roster.BitsMatchId, roster.Id), out var resultHeaderReadModel);
                var rosterCalendarEvent = new RosterCalendarEvent(roster, resultHeaderReadModel);
                calendarEvents.Add(rosterCalendarEvent);
            }

            return Request.CreateResponse(HttpStatusCode.OK, calendarEvents.ToArray(), new MediaTypeHeaderValue("text/iCal"));
        }
    }
}