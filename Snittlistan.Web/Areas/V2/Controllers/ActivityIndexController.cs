#nullable enable

namespace Snittlistan.Web.Areas.V2.Controllers
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Domain;
    using Indexes;
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

            Activity[] activities =
                DocumentSession.Query<Activity, ActivityIndex>()
                               .Where(x => x.Season >= season.Value)
                               .ToArray()
                               .OrderBy(x => x.Date)
                               .ToArray();

            IndexViewModel vm = new(activities);
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
                Id = activity.Id!;
                Title = activity.Title;
                Date = activity.Date;
                MessageHtml = new HtmlString(activity.MessageHtml);
            }

            public string Id { get; }

            public string Title { get; }

            public DateTime Date { get; }

            public IHtmlString MessageHtml { get; }
        }
    }
}
