using System;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventStoreLite;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Web.Areas.V2.Migration;
using Snittlistan.Web.Areas.V2.ReadModels;
using Snittlistan.Web.Areas.V2.ViewModels;
using Snittlistan.Web.Controllers;
using Snittlistan.Web.Helpers;
using Snittlistan.Web.Infrastructure.Indexes;
using Snittlistan.Web.Models;
using Snittlistan.Web.Tasks;

namespace Snittlistan.Web.Areas.V2.Controllers
{
    public class AdminTasksController : AdminController
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// GET: /User/Index.
        /// </summary>
        /// <returns></returns>
        public ActionResult Users()
        {
            var users = DocumentSession.Query<User>()
                .ToList()
                .Select(x => new UserViewModel(x))
                .OrderByDescending(x => x.IsActive)
                .ThenBy(x => x.Email)
                .ToList();

            return View(users);
        }

        public ActionResult CreateUser()
        {
            return View(new CreateUserViewModel());
        }

        [HttpPost]
        public ActionResult CreateUser(CreateUserViewModel vm)
        {
            // an existing user cannot be registered again
            if (DocumentSession.FindUserByEmail(vm.Email) != null)
                ModelState.AddModelError("Email", "Användaren finns redan");

            // redisplay form if any errors at this point
            if (!ModelState.IsValid) return View(vm);

            var newUser = new User(string.Empty, string.Empty, vm.Email, string.Empty);
            DocumentSession.Store(newUser);

            return RedirectToAction("Users");
        }

        public ActionResult DeleteUser(string id)
        {
            var user = DocumentSession.Load<User>(id);
            if (user == null) throw new HttpException(404, "User not found");

            return View(new UserViewModel(user));
        }

        [HttpPost]
        [ActionName("DeleteUser")]
        public ActionResult DeleteUserConfirmed(string id)
        {
            var user = DocumentSession.Load<User>(id);
            if (user == null) throw new HttpException(404, "User not found");
            DocumentSession.Delete(user);
            return RedirectToAction("Users");
        }

        public ActionResult ActivateUser(string id)
        {
            var user = DocumentSession.Load<User>(id);
            if (user == null) throw new HttpException(404, "User not found");

            return View(new UserViewModel(user));
        }

        [HttpPost]
        [ActionName("ActivateUser")]
        public ActionResult ActivateUserConfirmed(string id, bool? invite)
        {
            var user = DocumentSession.Load<User>(id);
            if (user == null) throw new HttpException(404, "User not found");
            if (user.IsActive) user.Deactivate();
            else
            {
                if (invite.GetValueOrDefault())
                {
                    Debug.Assert(Request.Url != null, "Request.Url != null");
                    user.ActivateWithEmail(Url, Request.Url.Scheme);
                }
                else
                {
                    user.Activate();
                }
            }

            return RedirectToAction("Users");
        }

        public ActionResult Raven()
        {
            return View();
        }

        public ActionResult ResetIndexes()
        {
            return View();
        }

        [HttpPost, ActionName("ResetIndexes")]
        public ActionResult ResetIndexesConfirmed()
        {
            IndexCreator.ResetIndexes(DocumentStore, EventStore);

            return RedirectToAction("Raven");
        }

        public ActionResult EventStoreActions()
        {
            return View();
        }

        public ActionResult ReplayEvents()
        {
            return View();
        }

        [HttpPost, ActionName("ReplayEvents")]
        public ActionResult ReplayEventsConfirmed()
        {
            EventStore.ReplayEvents(MvcApplication.ChildContainer);

            return RedirectToAction("EventStoreActions");
        }

        public ActionResult MigrateEvents()
        {
            return View("MigrateEvents", (HtmlString)null);
        }

        [HttpPost, ActionName("MigrateEvents")]
        public ActionResult MigrateEventsConfirmed()
        {
            var eventMigrators = MvcApplication.Container.ResolveAll<IEventMigratorWithResults>()
                .ToList();
            EventStore.MigrateEvents(eventMigrators);
            var results = string.Join("", eventMigrators.Select(x => x.GetResults()));

            return View("MigrateEvents", new HtmlString(results));
        }

        public ActionResult SendMail()
        {
            return View(new SendMailViewModel());
        }

        [HttpPost]
        public ActionResult SendMail(SendMailViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            SendTask(new EmailTask(vm.Recipient, vm.Subject, vm.Content));

            return RedirectToAction("Index");
        }

        public ActionResult AwardMedals()
        {
            return View();
        }

        [HttpPost, ActionName("AwardMedals")]
        public ActionResult AwardMedalsConfirmed()
        {
            var query = DocumentSession.Query<AggregateIdReadModel, AggregateIdIndex>();
            var current = 0;
            while (true)
            {
                var results = query.Skip(current)
                    .Take(10)
                    .ToArray();
                if (results.Length == 0) break;
                foreach (var result in results)
                {
                    if (result.Type == typeof(MatchResult))
                    {
                        var matchResult = EventStoreSession.Load<MatchResult>(result.AggregateId);
                        matchResult.ClearMedals();
                        matchResult.AwardMedals();
                    }
                    else if (result.Type == typeof(MatchResult4))
                    {
                        var matchResult4 = EventStoreSession.Load<MatchResult4>(result.AggregateId);
                        matchResult4.ClearMedals();
                        matchResult4.AwardMedals();
                    }
                }

                current += results.Length;
            }

            return RedirectToAction("Index");
        }

        public ActionResult AwardScores()
        {
            return View();
        }

        [HttpPost, ActionName("AwardScores")]
        public ActionResult AwardScoresConfirmed()
        {
            ForEachMatchResult(x => x.AwardScores());

            return RedirectToAction("Index");
        }

        private void ForEachMatchResult(Action<MatchResult> action)
        {
            var query = DocumentSession.Query<AggregateIdReadModel, AggregateIdIndex>();
            var current = 0;
            while (true)
            {
                var results = query.Skip(current)
                    .Take(30)
                    .ToArray();
                if (results.Length == 0) break;
                foreach (var result in results)
                {
                    if (result.Type == typeof(MatchResult))
                    {
                        var matchResult = EventStoreSession.Load<MatchResult>(result.AggregateId);
                        action.Invoke(matchResult);
                    }
                }

                current += results.Length;
            }
        }
    }
}