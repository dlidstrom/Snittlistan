#nullable enable

using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using Raven.Abstractions;
using Raven.Client;
using Raven.Client.Linq;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Web.Areas.V2.ViewModels;
using Snittlistan.Web.Controllers;

namespace Snittlistan.Web.Areas.V2.Controllers;

public class AbsenceController : AbstractController
{
    [Authorize(Roles = WebsiteRoles.Absence.View)]
    public ActionResult Index()
    {
        return View();
    }

    [ChildActionOnly]
    public ActionResult AbsenceTable(bool? hideControls)
    {
        ViewBag.HideControls = hideControls.GetValueOrDefault();
        AbsenceIndex.Result[] absences = CompositionRoot.DocumentSession.Query<AbsenceIndex.Result, AbsenceIndex>()
            .Where(x => x.To >= SystemTime.UtcNow.Date.AddDays(-1))
            .OrderBy(p => p.To)
            .ThenBy(p => p.PlayerName)
            .ProjectFromIndexFieldsInto<AbsenceIndex.Result>()
            .ToArray();
        return View(absences);
    }

    public ActionResult Create()
    {
        ViewBag.Player = CompositionRoot.DocumentSession.CreatePlayerSelectList();
        return View(new CreateAbsenceViewModel());
    }

    [HttpPost]
    [Authorize(Roles = WebsiteRoles.Uk.UkTasks)]
    public ActionResult Edit(int id, CreateAbsenceViewModel vm)
    {
        if (ModelState.IsValid == false)
        {
            ViewBag.Player = CompositionRoot.DocumentSession.CreatePlayerSelectList();
            return View(vm);
        }

        Absence absence = CompositionRoot.DocumentSession.Load<Absence>(id);
        absence.Player = vm.Player;
        absence.From = vm.From;
        absence.To = vm.To;
        absence.Comment = vm.Comment;
        return RedirectToAction("Index");
    }

    [Authorize(Roles = WebsiteRoles.Uk.UkTasks)]
    [HttpPost]
    public ActionResult Create(CreateAbsenceViewModel vm)
    {
        if (ModelState.IsValid == false)
        {
            ViewBag.Player = CompositionRoot.DocumentSession.CreatePlayerSelectList();
            return View(vm);
        }

        Absence absence = Absence.Create(vm.From, vm.To, vm.Player, vm.Comment);
        CompositionRoot.DocumentSession.Store(absence);

        return RedirectToAction("Index");
    }

    [Authorize(Roles = WebsiteRoles.Uk.UkTasks)]
    public ActionResult Edit(int id)
    {
        Absence absence = CompositionRoot.DocumentSession.Load<Absence>(id);
        if (absence == null)
        {
            throw new HttpException(404, "Absence not found");
        }

        ViewBag.Player = CompositionRoot.DocumentSession.CreatePlayerSelectList(absence.Player);
        return View(new CreateAbsenceViewModel(absence));
    }

    [Authorize(Roles = WebsiteRoles.Uk.UkTasks)]
    public ActionResult Delete(int id)
    {
        Absence absence = CompositionRoot.DocumentSession.Include<Absence>(x => x.Player).Load<Absence>(id);
        if (absence == null)
        {
            return RedirectToAction("Index");
        }

        Player player = CompositionRoot.DocumentSession.Load<Player>(absence.Player);
        return View(new AbsenceViewModel(absence, player));
    }

    [HttpPost]
    [Authorize(Roles = WebsiteRoles.Uk.UkTasks)]
    [ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
        Absence absence = CompositionRoot.DocumentSession.Load<Absence>(id);
        if (absence == null)
        {
            throw new HttpException(404, "Absence not found");
        }

        CompositionRoot.DocumentSession.Delete(absence);
        return RedirectToAction("Index");
    }

    public class CreateAbsenceViewModel : IValidatableObject
    {
        public CreateAbsenceViewModel()
        {
            Comment = string.Empty;
        }

        public CreateAbsenceViewModel(Absence absence)
        {
            DateRange =
                absence.From != absence.To
                ? $"{absence.From.ToShortDateString()} - {absence.To.ToShortDateString()}"
                : $"{absence.From.ToShortDateString()}";
            Player = absence.Player;
            Comment = absence.Comment;
        }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        [Required]
        [Display(Name = "Datum:")]
        public string DateRange { get; set; } = null!;

        [Required]
        public string Player { get; set; } = null!;

        public string Comment { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            string[] rangeSplit = DateRange.Split(new[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
            if (rangeSplit.Length > 0)
            {
                From = To = DateTime.Parse(rangeSplit[0]);
                if (rangeSplit.Length == 2)
                {
                    To = DateTime.Parse(rangeSplit[1]);
                }
            }

            if (To < SystemTime.UtcNow.Date)
            {
                yield return new ValidationResult("Till-datum kan inte vara passerade datum");
            }

            if (From > To)
            {
                yield return new ValidationResult("Från-datum kan inte vara före till-datum");
            }

            if (From.Year != To.Year)
            {
                yield return new ValidationResult("Frånvaro kan inte gå över flera år");
            }

            if (Comment != null && Comment.Length > 160)
            {
                yield return new ValidationResult("Kommentar kan vara högst 160 tecken");
            }
        }
    }
}
