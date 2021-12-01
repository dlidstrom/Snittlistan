
using System.Web.Mvc;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Controllers;

#nullable enable

namespace Snittlistan.Web.Areas.V2.Controllers;
[Authorize]
public class RosterAcceptController : AbstractController
{
    [HttpPost]
    public ActionResult Accept(string rosterId, string playerId, int season, int turn)
    {
        Roster roster = DocumentSession.Load<Roster>(rosterId);
        Roster.Update update = new(
            Roster.ChangeType.PlayerAccepted,
            User.CustomIdentity.PlayerId!)
        {
            PlayerAccepted = playerId
        };
        roster.UpdateWith(CorrelationId, update);
        return Redirect(
            Url.RouteUrl(
                new
                {
                    controller = "Roster",
                    action = "View",
                    season,
                    turn
                }) + $"#{rosterId}");
    }
}
