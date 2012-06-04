namespace Snittlistan.Controllers
{
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Helpers;
    using Infrastructure.AutoMapper;
    using Models;
    using MvcContrib;
    using Raven.Client;
    using ViewModels.Account;
    using ViewModels.Admin;

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
            var users = Session.Query<User>()
                .MapTo<UserViewModel>()
                .ToList();

            return View(users);
        }

        /// <summary>
        /// GET: /User/Edit/5.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            var user = Session.Load<User>(id);
            if (user == null)
                throw new HttpException(404, "User not found");

            return View(user.MapTo<EditUserViewModel>());
        }

        /// <summary>
        /// POST: /User/Edit.
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(EditUserViewModel vm)
        {
            var user = Session.Load<User>(vm.Id);
            if (user == null)
                throw new HttpException(404, "User not found");

            var newUser = new User(vm.FirstName, vm.LastName, vm.Email, vm.Password)
            {
                Id = vm.Id
            };

            if (vm.IsActive)
                newUser.Activate();

            Session.Advanced.Evict(user);
            Session.Store(newUser);

            return this.RedirectToAction(c => c.Index());
        }

        public ActionResult Details(string id)
        {
            var user = Session.Load<User>(id);
            if (user == null)
                throw new HttpException(404, "User not found");

            return View(user.MapTo<UserViewModel>());
        }

        public ActionResult Delete(string id)
        {
            var user = Session.Load<User>(id);
            if (user == null)
                throw new HttpException(404, "User not found");

            return View(user.MapTo<UserViewModel>());
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                // TODO: Add delete logic here
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Create()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public ActionResult Create(RegisterViewModel user)
        {
            // an existing user cannot be registered again
            if (Session.FindUserByEmail(user.Email) != null)
                ModelState.AddModelError("Email", "Användaren finns redan");

            // redisplay form if any errors at this point
            if (!ModelState.IsValid)
                return View(user);

            var newUser = new User(user.FirstName, user.LastName, user.Email, user.Password);
            Session.Store(newUser);

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