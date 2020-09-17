namespace Snittlistan.Web.Areas.V2.Controllers
{
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
            roster.Update(
                new Roster.Change(
                    Roster.ChangeType.PlayerAccepted,
                    User.Identity.Name,
                    playerAccepted: AuditLogEntry.PropertyChange.Create(null, playerId)));
            return RedirectToAction("View", "Roster", new { season, turn });
        }
    }
}
