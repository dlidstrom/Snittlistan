using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Raven.Abstractions;
using Raven.Client;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Indexes;

namespace Snittlistan.Web.Areas.V2
{
    public static class DocumentSessionExtensions
    {
        public static List<SelectListItem> CreateRosterSelectList(this IDocumentSession session, int season, string rosterId = "")
        {
            return session.Query<Roster, RosterSearchTerms>()
                          .Where(x => x.Season == season)
                          .Where(x => x.Preliminary == false)
                          .Where(x => x.BitsMatchId != 0)
                          .Where(x => x.Date < SystemTime.UtcNow)
                          .OrderBy(x => x.Date)
                          .ToList()
                          .Where(x => x.MatchResultId == null || string.IsNullOrEmpty(rosterId) == false)
                          .Select(
                              x => new SelectListItem
                              {
                                  Text = $"{x.Turn}: {x.Team} - {x.Opponent} ({x.Location} {x.Date.ToShortTimeString()})",
                                  Value = x.Id,
                                  Selected = x.Id == rosterId
                              })
                          .ToList();
        }
    }
}