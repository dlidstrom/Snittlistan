namespace Snittlistan.Web.Controllers
{
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using MvcContrib;

    using Raven.Client;

    using Snittlistan.Web.Helpers;
    using Snittlistan.Web.Infrastructure.AutoMapper;
    using Snittlistan.Web.Models;
    using Snittlistan.Web.ViewModels.Account;
    using Snittlistan.Web.ViewModels.Admin;

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
        { }

        /// <summary>
        /// GET: /User/Index.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var users = this.Session.Query<User>()
                .MapTo<UserViewModel>()
                .ToList();

            return this.View(users);
        }

        /// <summary>
        /// GET: /User/Edit/5.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            var user = this.Session.Load<User>(id);
            if (user == null)
                throw new HttpException(404, "User not found");

            return this.View(user.MapTo<EditUserViewModel>());
        }

        /// <summary>
        /// POST: /User/Edit.
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(EditUserViewModel vm)
        {
            var user = this.Session.Load<User>(vm.Id);
            if (user == null)
                throw new HttpException(404, "User not found");

            var newUser = new User(vm.FirstName, vm.LastName, vm.Email, vm.Password)
            {
                Id = vm.Id
            };

            if (vm.IsActive)
                newUser.Activate();

            this.Session.Advanced.Evict(user);
            this.Session.Store(newUser);

            return this.RedirectToAction(c => c.Index());
        }

        public ActionResult Details(string id)
        {
            var user = this.Session.Load<User>(id);
            if (user == null)
                throw new HttpException(404, "User not found");

            return this.View(user.MapTo<UserViewModel>());
        }

        public ActionResult Delete(string id)
        {
            var user = this.Session.Load<User>(id);
            if (user == null)
                throw new HttpException(404, "User not found");

            return this.View(user.MapTo<UserViewModel>());
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                // TODO: Add delete logic here
                return this.RedirectToAction("Index");
            }
            catch
            {
                return this.View();
            }
        }

        public ActionResult Create()
        {
            return this.View(new RegisterViewModel());
        }

        [HttpPost]
        public ActionResult Create(RegisterViewModel user)
        {
            // an existing user cannot be registered again
            if (this.Session.FindUserByEmail(user.Email) != null)
                this.ModelState.AddModelError("Email", "Användaren finns redan");

            // redisplay form if any errors at this point
            if (!this.ModelState.IsValid)
                return this.View(user);

            var newUser = new User(user.FirstName, user.LastName, user.Email, user.Password);
            this.Session.Store(newUser);

            return this.RedirectToAction(c => c.Index());
        }

        public ActionResult Activate(string id)
        {
            // todo: ajax
            return this.RedirectToAction(c => c.Index());
        }

        public ActionResult Deactivate(string id)
        {
            // todo: ajax
            return this.RedirectToAction(c => c.Index());
        }
    }
}