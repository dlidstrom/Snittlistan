#nullable enable

using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Helpers;
using Raven.Abstractions;
using Snittlistan.Web.Areas.V2.ViewModels;
using Snittlistan.Web.Controllers;

namespace Snittlistan.Web.Areas.V2.Controllers;

[Authorize(Roles = WebsiteRoles.Activity.Manage)]
public class ActivityEditController : AbstractController
{
    private const string DateTimeFormat = "yyyy-MM-dd HH:mm";

    public ActionResult Create(int? season)
    {
        if (season.HasValue == false)
        {
            season = CompositionRoot.DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);
        }

        return View(ActivityEditViewModel.ForCreate(season.Value));
    }

    [HttpPost]
    public ActionResult Create(ActivityEditViewModel vm)
    {
        if (ModelState.IsValid == false)
        {
            return View(vm);
        }

        Debug.Assert(vm.Season != null, "vm.Season != null");
        Debug.Assert(vm.Date != null, "vm.Date != null");
        Activity activity =
            Activity.Create(
                vm.Season!.Value,
                vm.Title,
                ParseDate(vm.Date!),
                vm.Message,
                vm.MessageHtml,
                User.CustomIdentity.PlayerId!);
        CompositionRoot.DocumentSession.Store(activity);

        return RedirectToAction("Index", "ActivityIndex");
    }

    public ActionResult Edit(string id)
    {
        Activity activity = CompositionRoot.DocumentSession.Load<Activity>(id);
        if (activity == null)
        {
            throw new HttpException(404, "Activity not found");
        }

        return View(ActivityEditViewModel.ForEdit(activity));
    }

    [HttpPost]
    public ActionResult Edit(string id, ActivityEditViewModel vm)
    {
        if (ModelState.IsValid == false)
        {
            return View(vm);
        }

        Activity activity = CompositionRoot.DocumentSession.Load<Activity>(id);
        if (activity == null)
        {
            throw new HttpException(404, "Activity not found");
        }

        Debug.Assert(vm.Season != null, "vm.Season != null");
        Debug.Assert(vm.Date != null, "vm.Date != null");
        activity.Update(
            vm.Season!.Value,
            vm.Title,
            ParseDate(vm.Date!),
            vm.Message,
            vm.MessageHtml,
            User.CustomIdentity.PlayerId!);
        return RedirectToAction("Index", "ActivityIndex");
    }

    private static DateTime ParseDate(string date)
    {
        return DateTime.ParseExact(date, DateTimeFormat, CultureInfo.InvariantCulture);
    }

    public ActionResult Delete(string id)
    {
        Activity activity = CompositionRoot.DocumentSession.Load<Activity>(id);
        if (activity == null)
        {
            throw new HttpException(404, "Activity not found");
        }

        Player player = CompositionRoot.DocumentSession.Load<Player>(activity.AuthorId);
        return View(new ActivityViewModel(activity.Id!, activity.Title, activity.Date, activity.Message, player?.Name ?? string.Empty));
    }

    [HttpPost]
    [ActionName("Delete")]
    public ActionResult DeleteActivity(string id)
    {
        Activity activity = CompositionRoot.DocumentSession.Load<Activity>(id);
        if (activity != null)
        {
            CompositionRoot.DocumentSession.Delete(activity);
        }

        return RedirectToAction("Index", "ActivityIndex");
    }

    public class ActivityEditViewModel
    {
        [Required]
        [Display(Name = "Säsong")]
        public int? Season { get; set; }

        [Required]
        [MaxLength(80)]
        [DataType(DataType.Text)]
        [Display(Name = "Titel")]
        public string Title { get; set; } = null!;

        [Required]
        [Display(Name = "Datum")]
        public string Date { get; set; } = null!;

        [Required]
        [MaxLength(10 * 1024)]
        public string Message { get; set; } = null!;

        [Required]
        [MaxLength(10 * 1024)]
        [AllowHtml]
        public string MessageHtml { get; set; } = null!;

        [Display(Name = "Meddelande")]
        public IHtmlString MessageDisplay { get; set; } = null!;

        public static ActivityEditViewModel ForCreate(int season)
        {
            return new ActivityEditViewModel
            {
                Season = season,
                Date = SystemTime.UtcNow.Date.AddDays(1).AddHours(18).ToString(DateTimeFormat)
            };
        }

        public static ActivityEditViewModel ForEdit(Activity activity)
        {
            return new ActivityEditViewModel
            {
                Season = activity.Season,
                Title = activity.Title,
                Date = activity.Date.ToString(DateTimeFormat),
                MessageDisplay = new HtmlString(activity.MessageHtml)
            };
        }
    }
}
