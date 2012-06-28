namespace Snittlistan.Controllers
{
    using System;
    using System.Configuration;
    using System.Threading;
    using System.Web.Mvc;
    using Common.Logging;
    using Models;
    using MvcContrib;
    using Raven.Client;
    using ViewModels.Account;

    public class WelcomeController : AbstractController
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        private const string MaintenanceAuthenticationTokenConstant = "Maintenance Authentication Token";

        public WelcomeController(IDocumentSession session)
            : base(session)
        { }

        /// <summary>
        /// Gets the maintenance authentication token.
        /// </summary>
        private string MaintenanceAuthenticationToken
        {
            get
            {
                if (HttpContext.Session != null && string.IsNullOrWhiteSpace((string)HttpContext.Session[MaintenanceAuthenticationTokenConstant]))
                {
                    var value = Guid.NewGuid().ToString();
                    HttpContext.Session[MaintenanceAuthenticationTokenConstant] = value;
                }

                return HttpContext.Session != null ? (string)HttpContext.Session[MaintenanceAuthenticationTokenConstant] : null;
            }
        }

        public ActionResult Index()
        {
            AssertAdminUserExists();

            var email = string.Format("admin@{0}", ConfigurationManager.AppSettings["Domain"]);
            var vm = new RegisterViewModel
            {
                Email = email,
                ConfirmEmail = email
            };

            return View(vm);
        }

        [HttpPost]
        public ActionResult CreateAdmin(RegisterViewModel adminUser)
        {
            AssertAdminUserExists();

            if (!ModelState.IsValid)
                return View("Index");

            var user = new User(
                adminUser.FirstName,
                adminUser.LastName,
                adminUser.Email,
                adminUser.Password)
                {
                    Id = "Admin"
                };
            user.Activate();
            Session.Store(user);

            return this.RedirectToAction(c => c.Success());
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Reset()
        {
            Log.InfoFormat("Maintenance Authentication Token: {0}", MaintenanceAuthenticationToken);
            return View(string.Empty);
        }

        [HttpPost]
        public ActionResult Reset(string token)
        {
            if (token != MaintenanceAuthenticationToken)
            {
                // wait a few seconds, to safeguard against attacks
                Thread.Sleep(5000);
                return View("Reset", null, token);
            }

            var user = Session.Load<User>("Admin");
            Session.Delete(user);
            Session.SaveChanges();

            // expect base controller to redirect to /welcome
            return this.RedirectToAction<HomeController>(c => c.Index());
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // don't load the non-existing admin user
        }

        private void AssertAdminUserExists()
        {
            if (Session.Load<User>("Admin") != null)
            {
                Response.Redirect("/");
                Response.End();
            }
        }
    }
}