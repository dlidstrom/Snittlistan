namespace Snittlistan.Web.Areas.V2.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Domain;
    using Raven.Abstractions;
    using Snittlistan.Web.Helpers;
    using Web.Controllers;

    [Authorize(Roles = WebsiteRoles.Activity.Manage)]
    public class ActivityIndexController : AbstractController
    {
        public ActionResult Index(int? season)
        {
            if (season.HasValue == false)
            {
                season = DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);
            }

            //var activities =
            //    DocumentSession.Query<Activity, ActivityIndex>()
            //                   .Where(x => x.Season == season.Value)
            //                   .ToArray();

            var activities = new[]
            {
                Activity.Create(
                    2018,
                    "Träning",
                    new DateTime(2019, 2, 27, 18, 0, 0),
                    "Ett meddelande till allaEtt meddelande till allaEtt meddelande till allaEtt meddelande till allaEtt meddelande till alla")
            };

            var vm = new IndexViewModel(activities);
            return View(vm);
        }

        public class IndexViewModel
        {
            public IndexViewModel(Activity[] activities)
            {
                Items = activities.Select(x => new IndexItemViewModel(x)).ToArray();
            }

            public IndexItemViewModel[] Items { get; }
        }

        public class IndexItemViewModel
        {
            public IndexItemViewModel(Activity activity)
            {
                Title = activity.Title;
                Date = activity.Date;
                Message = activity.Message;
            }

            public string Title { get; }

            public DateTime Date { get; }

            public string Message { get; }
        }
    }
}