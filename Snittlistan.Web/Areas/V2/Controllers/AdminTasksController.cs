using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventStoreLite;
using EventStoreLite.IoC.Castle;
using Raven.Client;
using Snittlistan.Web.Areas.V2.ViewModels;
using Snittlistan.Web.Controllers;
using Snittlistan.Web.Helpers;
using Snittlistan.Web.Infrastructure.AutoMapper;
using Snittlistan.Web.Infrastructure.Indexes;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.Areas.V2.Controllers
{
    public class AdminTasksController : AdminController
    {
        private readonly IDocumentStore store;

        public AdminTasksController(IDocumentStore store)
        {
            this.store = store;
        }

        public ActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// GET: /User/Index.
        /// </summary>
        /// <returns></returns>
        public ActionResult Users()
        {
            var users = this.DocumentSession.Query<User>()
                .ToList()
                .MapTo<UserViewModel>()
                .OrderByDescending(x => x.IsActive)
                .ThenBy(x => x.Email)
                .ToList();

            return this.View(users);
        }

        public ActionResult CreateUser()
        {
            return this.View(new CreateUserViewModel());
        }

        [HttpPost]
        public ActionResult CreateUser(CreateUserViewModel vm)
        {
            // an existing user cannot be registered again
            if (this.DocumentSession.FindUserByEmail(vm.Email) != null)
                this.ModelState.AddModelError("Email", "Användaren finns redan");

            // redisplay form if any errors at this point
            if (!this.ModelState.IsValid) return this.View(vm);

            var newUser = new User(string.Empty, string.Empty, vm.Email, string.Empty);
            this.DocumentSession.Store(newUser);

            return this.RedirectToAction("Users");
        }

        public ActionResult DeleteUser(string id)
        {
            var user = this.DocumentSession.Load<User>(id);
            if (user == null) throw new HttpException(404, "User not found");

            return this.View(user.MapTo<UserViewModel>());
        }

        [HttpPost]
        [ActionName("DeleteUser")]
        public ActionResult DeleteUserConfirmed(string id)
        {
            var user = this.DocumentSession.Load<User>(id);
            if (user == null) throw new HttpException(404, "User not found");
            DocumentSession.Delete(user);
            return this.RedirectToAction("Users");
        }

        public ActionResult ActivateUser(string id)
        {
            var user = this.DocumentSession.Load<User>(id);
            if (user == null) throw new HttpException(404, "User not found");

            return this.View(user.MapTo<UserViewModel>());
        }

        [HttpPost]
        [ActionName("ActivateUser")]
        public ActionResult ActivateUserConfirmed(string id, bool? invite)
        {
            var user = this.DocumentSession.Load<User>(id);
            if (user == null) throw new HttpException(404, "User not found");
            if (user.IsActive) user.Deactivate();
            else
            {
                user.Activate(invite.HasValue && invite.Value);
            }

            return this.RedirectToAction("Users");
        }

        public ActionResult Raven()
        {
            return this.View();
        }

        public ActionResult CreateIndexes()
        {
            return this.View();
        }

        [HttpPost, ActionName("CreateIndexes")]
        public ActionResult CreateIndexesConfirmed()
        {
            // create indexes
            IndexCreator.CreateIndexes(store);

            return this.RedirectToAction("Raven");
        }

        public ActionResult ResetIndexes()
        {
            return this.View();
        }

        [HttpPost, ActionName("ResetIndexes")]
        public ActionResult ResetIndexesConfirmed()
        {
            while (true)
            {
                var indexNames = store.DatabaseCommands.GetIndexNames(0, 20);
                if (indexNames.Length == 0) break;
                foreach (var indexName in indexNames)
                {
                    store.DatabaseCommands.DeleteIndex(indexName);
                }
            }

            // create indexes
            IndexCreator.CreateIndexes(store);
            EventStore.Initialize(DocumentStore);

            return this.RedirectToAction("Raven");
        }

        public ActionResult EventStoreActions()
        {
            return this.View();
        }

        public ActionResult ReplayEvents()
        {
            return this.View();
        }

        [HttpPost, ActionName("ReplayEvents")]
        public ActionResult ReplayEventsConfirmed()
        {
            var locator = new WindsorServiceLocator(MvcApplication.ChildContainer);
            EventStore.ReplayEvents(locator);

            return this.RedirectToAction("EventStoreActions");
        }

        public ActionResult MigrateEvents()
        {
            return this.View();
        }

        [HttpPost, ActionName("MigrateEvents")]
        public ActionResult MigrateEventsConfirmed()
        {
            var eventMigrators = MvcApplication.Container.ResolveAll<IEventMigrator>()
                .ToList();
            EventStore.MigrateEvents(eventMigrators);

            return this.RedirectToAction("EventStoreActions");
        }

        public ActionResult SendMail()
        {
            return this.View(new SendMailViewModel());
        }

        [HttpPost]
        public ActionResult SendMail(SendMailViewModel vm)
        {
            if (!ModelState.IsValid) return this.View(vm);

            Emails.SendMail(vm.Recipient, vm.Subject, vm.Content);

            return this.RedirectToAction("Index");
        }
    }
}
