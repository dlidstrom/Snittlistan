using System.Linq;
using System.Web;
using System.Web.Mvc;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Web.Areas.V2.ViewModels;
using Snittlistan.Web.Controllers;

namespace Snittlistan.Web.Areas.V2.Controllers
{
    public class PlayerController : AbstractController
    {
        public ActionResult Index()
        {
            var players = DocumentSession.Query<Player, PlayerSearch>()
                .OrderBy(p => p.PlayerStatus)
                .ThenBy(p => p.Name)
                .ToList();
            var vm = players.Select(x => new PlayerViewModel(x)).ToList();
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
            // prevent duplicates
            var isDuplicate = DocumentSession.Query<Player, PlayerSearch>().Any(x => x.Name == vm.Name);
            if (isDuplicate)
            {
                ModelState.AddModelError("namn", "Namnet är redan registrerat");
            }

            if (!ModelState.IsValid) return View(vm);

            var player = new Player(vm.Name, vm.Email, vm.Status, vm.PersonalNumber.GetValueOrDefault(), vm.Nickname);
            DocumentSession.Store(player);
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var player = DocumentSession.Load<Player>(id);
            if (player == null) throw new HttpException(404, "Player not found");
            return View(new CreatePlayerViewModel(player));
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(int id, CreatePlayerViewModel vm)
        {
            var player = DocumentSession.Load<Player>(id);
            if (player == null) throw new HttpException(404, "Player not found");

            // prevent duplicates
            var duplicates = DocumentSession.Query<Player, PlayerSearch>().Where(x => x.Name == vm.Name).ToArray();
            if (duplicates.Any(x => x.Id != player.Id))
            {
                ModelState.AddModelError("namn", "Namnet är redan registrerat");
            }

            if (!ModelState.IsValid)
                return View(vm);

            player.SetName(vm.Name);
            player.SetEmail(vm.Email);
            player.SetStatus(vm.Status);
            player.SetPersonalNumber(vm.PersonalNumber.GetValueOrDefault());
            player.SetNickname(vm.Nickname);

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var player = DocumentSession.Load<Player>(id);
            if (player == null)
                throw new HttpException(404, "Player not found");
            return View(new PlayerViewModel(player));
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