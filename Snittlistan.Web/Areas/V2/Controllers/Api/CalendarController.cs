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

namespace Snittlistan.Web.Areas.V2.Controllers.Api
{
    public class CalendarController : AbstractApiController
    {
        public HttpResponseMessage Get()
        {
            var season = DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);

            var rosters = DocumentSession.Query<Roster, RosterSearchTerms>()
                                         .Where(r => r.Season == season)
                                         .ToArray();
            var rosterDictionary = rosters.ToDictionary(x => x.Id);
            var resultIds = rosters.Select(x => ResultHeaderReadModel.IdFromBitsMatchId(x.BitsMatchId));
            var results = DocumentSession.Load<ResultHeaderReadModel>(resultIds);
            var resultsDictionary = results.Where(x => x != null).ToDictionary(x => rosterDictionary[x.RosterId].BitsMatchId);
            var calendarEvents = new List<RosterCalendarEvent>();
            foreach (var roster in rosters)
            {
                var players = DocumentSession.Load<Player>(roster.Players);
                resultsDictionary.TryGetValue(roster.BitsMatchId, out var resultHeaderReadModel);
                Player teamLeader = null;
                if (roster.TeamLeader != null)
                {
                    teamLeader = DocumentSession.Load<Player>(roster.TeamLeader);
                }

                var rosterCalendarEvent = new RosterCalendarEvent(roster, players, teamLeader, resultHeaderReadModel);
                calendarEvents.Add(rosterCalendarEvent);
            }

            return Request.CreateResponse(HttpStatusCode.OK, calendarEvents.ToArray(), new MediaTypeHeaderValue("text/iCal"));
        }
    }
}