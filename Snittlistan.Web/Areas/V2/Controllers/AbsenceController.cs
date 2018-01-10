using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raven.Abstractions;
using Raven.Client;
using Raven.Client.Linq;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Web.Areas.V2.ViewModels;
using Snittlistan.Web.Controllers;

namespace Snittlistan.Web.Areas.V2.Controllers
{
    public class AbsenceController : AbstractController
    {
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult AbsenceTable(bool? hideControls)
        {
            ViewBag.HideControls = hideControls.GetValueOrDefault();
            var absences = DocumentSession.Query<AbsenceIndex.Result, AbsenceIndex>()
                                          .Where(x => x.To >= SystemTime.UtcNow.Date.AddDays(-1))
                                          .OrderBy(p => p.To)
                                          .ThenBy(p => p.PlayerName)
                                          .ProjectFromIndexFieldsInto<AbsenceIndex.Result>()
                                          .ToArray();
            return View(absences);
        }

        public ActionResult Create()
        {
            CreatePlayerSelectList();
            return View(new CreateAbsenceViewModel());
        }

        [Authorize, HttpPost]
        public ActionResult Create(CreateAbsenceViewModel vm)
        {
            if (ModelState.IsValid == false)
            {
                CreatePlayerSelectList();
                return View(vm);
            }

            var absence = Absence.Create(vm.From, vm.To, vm.Player, vm.Comment);
            DocumentSession.Store(absence);

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var absence = DocumentSession.Load<Absence>(id);
            if (absence == null) throw new HttpException(404, "Absence not found");
            CreatePlayerSelectList(absence.Player);
            return View(new CreateAbsenceViewModel(absence));
        }

        [HttpPost, Authorize]
        public ActionResult Edit(int id, CreateAbsenceViewModel vm)
        {
            if (ModelState.IsValid == false)
            {
                CreatePlayerSelectList();
                return View(vm);
            }

            var absence = DocumentSession.Load<Absence>(id);
            absence.Player = vm.Player;
            absence.From = vm.From;
            absence.To = vm.To;
            absence.Comment = vm.Comment;
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var absence = DocumentSession.Include<Absence>(x => x.Player).Load<Absence>(id);
            if (absence == null) throw new HttpException(404, "Absence not found");
            var player = DocumentSession.Load<Player>(absence.Player);
            return View(new AbsenceViewModel(absence, player));
        }

        [HttpPost, Authorize, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var absence = DocumentSession.Load<Absence>(id);
            if (absence == null) throw new HttpException(404, "Absence not found");
            DocumentSession.Delete(absence);
            return RedirectToAction("Index");
        }

        private void CreatePlayerSelectList(string player = "")
        {
            var playerList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Välj medlem",
                    Value = string.Empty
                }
            };
            var players = DocumentSession.Query<Player, PlayerSearch>()
                                         .Where(x => x.PlayerStatus == Player.Status.Active)
                                         .OrderBy(x => x.Name)
                                         .ToList()
                                         .Select(x => new SelectListItem
                                         {
                                             Text = x.Name,
                                             Value = x.Id,
                                             Selected = x.Id == player
                                         })
                                         .ToArray();
            playerList.AddRange(players);
            ViewBag.Player = playerList;
        }
    }
}