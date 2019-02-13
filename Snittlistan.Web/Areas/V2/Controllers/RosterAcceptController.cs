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
            var roster = DocumentSession.Load<Roster>(rosterId);
            roster.Accept(playerId);
            return RedirectToAction("View", "Roster", new { season, turn });
        }
    }
}