namespace Snittlistan.Web.Areas.V2.Controllers
{
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using MvcContrib;

    using Raven.Client;

    using Snittlistan.Web.Areas.V1.Controllers;
    using Snittlistan.Web.Areas.V2.ViewModels;
    using Snittlistan.Web.Helpers;
    using Snittlistan.Web.Infrastructure.AutoMapper;
    using Snittlistan.Web.Models;

    /// <summary>
    /// User administration.
    /// </summary>
    public class UserController : AdminController
    {
        /// <summary>
        /// Initializes a new instance of the UserController class.
        /// </summary>
        /// <param name="session">Document session.</param>
        public UserController(IDocumentSession session)
            : base(session)
        {
        }

        /// <summary>
        /// GET: /User/Index.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var users = this.Session.Query<User>()
                .MapTo<UserViewModel>()
                .OrderByDescending(x => x.IsActive)
                .ThenBy(x => x.Email)
                .ToList();

            return this.View(users);
        }

        public ActionResult Delete(string id)
        {
            var user = this.Session.Load<User>(id);
            if (user == null) throw new HttpException(404, "User not found");

            return this.View(user.MapTo<UserViewModel>());
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            var user = this.Session.Load<User>(id);
            if (user == null) throw new HttpException(404, "User not found");
            Session.Delete(user);
            return this.RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return this.View(new CreateUserViewModel());
        }

        [HttpPost]
        public ActionResult Create(CreateUserViewModel vm)
        {
            // an existing user cannot be registered again
            if (this.Session.FindUserByEmail(vm.Email) != null)
            {
                this.ModelState.AddModelError("Email", "Användaren finns redan");
            }

            // redisplay form if any errors at this point
            if (!this.ModelState.IsValid) return this.View(vm);

            var newUser = new User(string.Empty, string.Empty, vm.Email, string.Empty);
            this.Session.Store(newUser);

            return this.RedirectToAction(c => c.Index());
        }

        public ActionResult Activate(string id)
        {
            var user = this.Session.Load<User>(id);
            if (user == null) throw new HttpException(404, "User not found");

            return this.View(user.MapTo<UserViewModel>());
        }

        [HttpPost]
        [ActionName("Activate")]
        public ActionResult ActivateConfirmed(string id, bool? invite)
        {
            var user = this.Session.Load<User>(id);
            if (user == null) throw new HttpException(404, "User not found");
            if (user.IsActive) user.Deactivate();
            else
            {
                user.Activate(invite.HasValue && invite.Value);
            }

            return this.RedirectToAction("Index");
        }
    }
}