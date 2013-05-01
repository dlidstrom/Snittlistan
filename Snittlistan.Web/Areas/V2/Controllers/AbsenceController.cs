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
using Snittlistan.Web.Infrastructure.AutoMapper;

namespace Snittlistan.Web.Areas.V2.Controllers
{
    public class AbsenceController : AbstractController
    {
        public ActionResult Index()
        {
            var absences = DocumentSession.Query<AbsenceIndex.Result, AbsenceIndex>()
                .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                .Where(x => x.To >= SystemTime.UtcNow.Date.AddDays(-1))
                .OrderBy(p => p.From)
                .ThenBy(p => p.To)
                .AsProjection<AbsenceIndex.Result>()
                .ToList();
            return View(absences);
        }

        public ActionResult Create()
        {
            this.CreatePlayerSelectList();
            return View(new CreateAbsenceViewModel());
        }

        [Authorize, HttpPost]
        public ActionResult Create(CreateAbsenceViewModel vm)
        {
            if (vm.From < SystemTime.UtcNow.Date.AddDays(1))
                ModelState.AddModelError("From", "Kan inte välja datum före idag");
            if (vm.To < SystemTime.UtcNow.Date.AddDays(1))
                ModelState.AddModelError("From", "Kan inte välja datum före idag");
            if (vm.From > vm.To)
                ModelState.AddModelError("From", "Från-datum kan inte vara före till-datum");

            if (ModelState.IsValid == false)
            {
                this.CreatePlayerSelectList();
                return this.View(vm);
            }

            var absence = new Absence(vm.From, vm.To, vm.Player);
            DocumentSession.Store(absence);

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var absence = DocumentSession.Load<Absence>(id);
            if (absence == null) throw new HttpException(404, "Absence not found");
            this.CreatePlayerSelectList(absence.Player);
            return View(absence.MapTo<CreateAbsenceViewModel>());
        }

        [HttpPost, Authorize]
        public ActionResult Edit(int id, CreateAbsenceViewModel vm)
        {
            if (vm.From < SystemTime.UtcNow.Date.AddDays(1))
                ModelState.AddModelError("From", "Kan inte välja datum före idag");
            if (vm.To < SystemTime.UtcNow.Date.AddDays(1))
                ModelState.AddModelError("From", "Kan inte välja datum före idag");
            if (vm.From > vm.To)
                ModelState.AddModelError("From", "Från-datum kan inte vara före till-datum");

            if (ModelState.IsValid == false)
            {
                this.CreatePlayerSelectList();
                return this.View(vm);
            }

            var absence = DocumentSession.Load<Absence>(id);
            absence.Player = vm.Player;
            absence.From = vm.From;
            absence.To = vm.To;
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var absence = DocumentSession.Load<Absence>(id);
            if (absence == null) throw new HttpException(404, "Absence not found");
            return this.View(absence.MapTo<AbsenceViewModel>());
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
            this.ViewBag.Player = this.DocumentSession.Query<Player, PlayerSearch>()
                .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                .Where(x => x.IsSupporter == false)
                .OrderBy(x => x.Name)
                .ToList()
                .Select(
                    x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id,
                        Selected = x.Id == player
                    })
                .ToList();
        }
    }
}