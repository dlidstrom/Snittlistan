namespace Snittlistan.Web.Areas.V2.Controllers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Web;
    using System.Web.Mvc;
    using Domain;
    using Helpers;
    using Raven.Abstractions;
    using Web.Controllers;

    [Authorize(Roles = WebsiteRoles.Activity.Manage)]
    public class ActivityEditController : AbstractController
    {
        public ActionResult Create(int? season)
        {
            if (season.HasValue == false)
                season = DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);
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
            var activity = Activity.Create(vm.Season.Value, vm.Title, vm.Date.Value, vm.Message);
            DocumentSession.Store(activity);

            return RedirectToAction("Index", "ActivityIndex");
        }

        public ActionResult Edit(string id)
        {
            var activity = DocumentSession.Load<Activity>(id);
            if (activity == null) throw new HttpException(404, "Activity not found");
            return View(ActivityEditViewModel.ForEdit(activity));
        }

        [HttpPost]
        public ActionResult Edit(string id, ActivityEditViewModel vm)
        {
            if (ModelState.IsValid == false) return View(vm);
            var activity = DocumentSession.Load<Activity>(id);
            Debug.Assert(vm.Season != null, "vm.Season != null");
            Debug.Assert(vm.Date != null, "vm.Date != null");
            activity.Update(vm.Season.Value, vm.Title, vm.Date.Value, vm.Message);
            return RedirectToAction("Index", "ActivityIndex");
        }

        public ActionResult Delete(string id)
        {
            return View();
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
            public string Title { get; set; }

            [Required]
            [DataType(DataType.DateTime)]
            [Display(Name = "Datum")]
            public DateTime? Date { get; set; }

            [Required]
            [MaxLength(1024)]
            [DataType(DataType.MultilineText)]
            [Display(Name = "Meddelande")]
            public string Message { get; set; }

            public static ActivityEditViewModel ForCreate(int season)
            {
                return new ActivityEditViewModel
                {
                    Season = season
                };
            }

            public static ActivityEditViewModel ForEdit(Activity activity)
            {
                return new ActivityEditViewModel
                {
                    Season = activity.Season,
                    Title = activity.Title,
                    Date = activity.Date,
                    Message = activity.Message
                };
            }
        }
    }
}