namespace Snittlistan.Web.Areas.V2.Controllers
{
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using Raven.Client;

    using Snittlistan.Web.Areas.V2.ViewModels;
    using Snittlistan.Web.Controllers;
    using Snittlistan.Web.Helpers;
    using Snittlistan.Web.Infrastructure.AutoMapper;
    using Snittlistan.Web.Infrastructure.Indexes;
    using Snittlistan.Web.Models;

    public class AdminTasksController : AdminController
    {
        private readonly IDocumentStore store;

        public AdminTasksController(IDocumentSession session, IDocumentStore store)
            : base(session)
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
            var users = this.Session.Query<User>()
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
            if (this.Session.FindUserByEmail(vm.Email) != null)
                this.ModelState.AddModelError("Email", "Användaren finns redan");

            // redisplay form if any errors at this point
            if (!this.ModelState.IsValid) return this.View(vm);

            var newUser = new User(string.Empty, string.Empty, vm.Email, string.Empty);
            this.Session.Store(newUser);

            return this.RedirectToAction("Users");
        }

        public ActionResult DeleteUser(string id)
        {
            var user = this.Session.Load<User>(id);
            if (user == null) throw new HttpException(404, "User not found");

            return this.View(user.MapTo<UserViewModel>());
        }

        [HttpPost]
        [ActionName("DeleteUser")]
        public ActionResult DeleteUserConfirmed(string id)
        {
            var user = this.Session.Load<User>(id);
            if (user == null) throw new HttpException(404, "User not found");
            Session.Delete(user);
            return this.RedirectToAction("Users");
        }

        public ActionResult ActivateUser(string id)
        {
            var user = this.Session.Load<User>(id);
            if (user == null) throw new HttpException(404, "User not found");

            return this.View(user.MapTo<UserViewModel>());
        }

        [HttpPost]
        [ActionName("ActivateUser")]
        public ActionResult ActivateUserConfirmed(string id, bool? invite)
        {
            var user = this.Session.Load<User>(id);
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

        public ActionResult ResetIndexes()
        {
            return this.View();
        }

        [HttpPost, ActionName("ResetIndexes")]
        public ActionResult ResetIndexesConfirmed()
        {
            var indexNames = store.DatabaseCommands.GetIndexNames(0, 20);
            while (indexNames.Length > 0)
            {
                foreach (var indexName in indexNames)
                {
                    store.DatabaseCommands.DeleteIndex(indexName);
                }

                indexNames = store.DatabaseCommands.GetIndexNames(0, 20);
            }

            // create indexes
            IndexCreator.CreateIndexes(store);

            return this.RedirectToAction("Raven");
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
