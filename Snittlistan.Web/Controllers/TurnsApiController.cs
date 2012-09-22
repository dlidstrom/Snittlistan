namespace Snittlistan.Web.Controllers
{
    using System.Web.Mvc;

    using Raven.Client;

    using Snittlistan.Web.Infrastructure.AutoMapper;
    using Snittlistan.Web.Models;
    using Snittlistan.Web.ViewModels;

    public class TurnsApiController : ApiController
    {
        public TurnsApiController(IDocumentSession session) : base(session)
        {
        }

        [HttpPost]
        [ActionName("Turns")]
        public ActionResult Create(TurnViewModel vm)
        {
            var turn = new TurnModel(vm.Turn, vm.Team, vm.Opponent, vm.Location, vm.Date, vm.Time);
            this.Session.Store(turn);
            return this.Json(turn.MapTo<TurnViewModel>());
        }
    }
}
