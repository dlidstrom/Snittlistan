namespace Snittlistan.Web.Areas.V2.Controllers
{
    using System.Diagnostics;
    using System.Web.Mvc;
    using Domain;
    using Web.Controllers;

    [Authorize]
    public class RosterAcceptController : AbstractController
    {
        [HttpPost]
        public ActionResult Accept(string rosterId, string playerId, int season, int turn)
        {
            Roster roster = DocumentSession.Load<Roster>(rosterId);
            var update = new Roster.Update(
                Roster.ChangeType.PlayerAccepted,
                User.CustomIdentity.PlayerId)
            {
                PlayerAccepted = playerId
            };
            roster.UpdateWith(Trace.CorrelationManager.ActivityId, update);
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
}
