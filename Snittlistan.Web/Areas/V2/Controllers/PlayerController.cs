namespace Snittlistan.Web.Areas.V2.Controllers
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using Raven.Abstractions;
    using Raven.Client;

    using Snittlistan.Web.Areas.V2.Models;
    using Snittlistan.Web.Areas.V2.ViewModels;
    using Snittlistan.Web.Controllers;
    using Snittlistan.Web.Helpers;
    using Snittlistan.Web.Infrastructure.AutoMapper;

    public class PlayerController : AbstractController
    {
        public PlayerController(IDocumentSession session)
            : base(session)
        {
            if (session == null) throw new ArgumentNullException("session");
        }

        public ActionResult Index(int? season)
        {
            if (season.HasValue == false)
                season = this.Session.LatestSeasonOrDefault(SystemTime.UtcNow.Year);
            var players = Session.Query<Player>()
                .OrderBy(p => p.IsSupporter)
                .ThenBy(p => p.Name)
                .ToList();
            var vm = new PlayerDataViewModel
            {
                Players = players.MapTo<PlayerViewModel>(),
                SeasonStart = season.Value,
                SeasonEnd = season.Value + 1
            };
            return View(vm);
        }

        [Authorize]
        public ActionResult Create()
        {
            return this.View(new CreatePlayerViewModel());
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(CreatePlayerViewModel vm)
        {
            if (!this.ModelState.IsValid) return this.View(vm);

            var player = new Player(vm.Name, vm.Email, vm.IsSupporter);
            this.Session.Store(player);
            return this.RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var player = Session.Load<Player>(id);
            if (player == null) throw new HttpException(404, "Player not found");
            return this.View(player.MapTo<CreatePlayerViewModel>());
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(int id, CreatePlayerViewModel vm)
        {
            if (!this.ModelState.IsValid)
                return this.View(vm);

            var player = this.Session.Load<Player>(id);
            if (player == null) throw new HttpException(404, "Player not found");

            player.SetName(vm.Name);
            player.SetEmail(vm.Email);
            player.SetIsSupporter(vm.IsSupporter);

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var player = this.Session.Load<Player>(id);
            if (player == null)
                throw new HttpException(404, "Player not found");
            return this.View(player.MapTo<PlayerViewModel>());
        }

        [HttpPost]
        [Authorize]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var player = this.Session.Load<Player>(id);
            if (player == null)
                throw new HttpException(404, "Player not found");
            this.Session.Delete(player);
            return this.RedirectToAction("Index");
        }
    }
}
