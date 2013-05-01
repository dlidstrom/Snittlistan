using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raven.Client.Linq;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.ViewModels;
using Snittlistan.Web.Controllers;
using Snittlistan.Web.Infrastructure.AutoMapper;

namespace Snittlistan.Web.Areas.V2.Controllers
{
    public class PlayerController : AbstractController
    {
        public ActionResult Index()
        {
            var players = DocumentSession.Query<Player>()
                .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                .OrderBy(p => p.IsSupporter)
                .ThenBy(p => p.Name)
                .ToList();
            var vm = players.MapTo<PlayerViewModel>();
            return View(vm);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View(new CreatePlayerViewModel());
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(CreatePlayerViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var player = new Player(vm.Name, vm.Email, vm.IsSupporter);
            DocumentSession.Store(player);
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var player = DocumentSession.Load<Player>(id);
            if (player == null) throw new HttpException(404, "Player not found");
            return View(player.MapTo<CreatePlayerViewModel>());
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(int id, CreatePlayerViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var player = DocumentSession.Load<Player>(id);
            if (player == null) throw new HttpException(404, "Player not found");

            player.SetName(vm.Name);
            player.SetEmail(vm.Email);
            player.SetIsSupporter(vm.IsSupporter);

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var player = DocumentSession.Load<Player>(id);
            if (player == null)
                throw new HttpException(404, "Player not found");
            return View(player.MapTo<PlayerViewModel>());
        }

        [HttpPost]
        [Authorize]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var player = DocumentSession.Load<Player>(id);
            if (player == null)
                throw new HttpException(404, "Player not found");
            DocumentSession.Delete(player);
            return RedirectToAction("Index");
        }
    }
}
