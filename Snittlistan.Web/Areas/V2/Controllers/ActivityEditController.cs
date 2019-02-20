namespace Snittlistan.Web.Areas.V2.Controllers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Web.Mvc;
    using Domain;
    using Web.Controllers;

    [Authorize(Roles = WebsiteRoles.Activity.Manage)]
    public class ActivityEditController : AbstractController
    {
        public ActionResult Create()
        {
            return View(new ActivityViewModel());
        }

        [HttpPost]
        public ActionResult Create(ActivityViewModel vm)
        {
            if (ModelState.IsValid == false)
            {
                return View(vm);
            }

            Debug.Assert(vm.Season != null, "vm.Season != null");
            Debug.Assert(vm.Date != null, "vm.Date != null");
            var activity = Activity.Create(vm.Season.Value, vm.Title, vm.Date.Value, vm.Message);

            return RedirectToAction("Index", "ActivityIndex");
        }

        public ActionResult Edit(string id)
        {
            return View();
        }

        public ActionResult Delete(string id)
        {
            return View();
        }

        public class ActivityViewModel
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
        }
    }
}