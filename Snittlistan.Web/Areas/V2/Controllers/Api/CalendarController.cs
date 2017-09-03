using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Raven.Abstractions;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Indexes;
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
                                         .ToArray()
                                         .Select(x => new RosterCalendarEvent(x, DocumentSession.Load<Player>(x.Players)))
                                         .ToArray();
            return Request.CreateResponse(HttpStatusCode.OK, rosters, new MediaTypeHeaderValue("text/iCal"));
        }
    }
}