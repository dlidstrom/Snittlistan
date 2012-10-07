namespace Snittlistan.Web.Areas.V1.Controllers
{
    using System;
    using System.Configuration;
    using System.Threading;
    using System.Web.Mvc;

    using MvcContrib;

    using NLog;

    using Raven.Client;

    using Snittlistan.Web.Areas.V1.ViewModels.Account;
    using Snittlistan.Web.Controllers;
    using Snittlistan.Web.Models;

    public class WelcomeController : AbstractController
    {
        private const string MaintenanceAuthenticationTokenConstant = "Maintenance Authentication Token";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

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
                if (this.HttpContext.Session != null && string.IsNullOrWhiteSpace((string)this.HttpContext.Session[MaintenanceAuthenticationTokenConstant]))
                {
                    var value = Guid.NewGuid().ToString();
                    this.HttpContext.Session[MaintenanceAuthenticationTokenConstant] = value;
                }

                return this.HttpContext.Session != null ? (string)this.HttpContext.Session[MaintenanceAuthenticationTokenConstant] : null;
            }
        }

        public ActionResult Index()
        {
            this.AssertAdminUserExists();

            var email = string.Format("admin@{0}", ConfigurationManager.AppSettings["Domain"]);
            var vm = new RegisterViewModel
            {
                Email = email,
                ConfirmEmail = email
            };

            return this.View(vm);
        }

        [HttpPost]
        public ActionResult CreateAdmin(RegisterViewModel adminUser)
        {
            this.AssertAdminUserExists();

            if (!this.ModelState.IsValid)
                return this.View("Index");

            var user = new User(
                adminUser.FirstName,
                adminUser.LastName,
                adminUser.Email,
                adminUser.Password)
                {
                    Id = "Admin"
                };
            user.Activate();
            this.Session.Store(user);

            return this.RedirectToAction(c => c.Success());
        }

        public ActionResult Success()
        {
            return this.View();
        }

        public ActionResult Reset()
        {
            Log.Info("Maintenance Authentication Token: {0}", this.MaintenanceAuthenticationToken);
            return this.View(string.Empty);
        }

        [HttpPost]
        public ActionResult Reset(string token)
        {
            if (token != this.MaintenanceAuthenticationToken)
            {
                // wait a few seconds, to safeguard against attacks
                Thread.Sleep(5000);
                return this.View("Reset", null, token);
            }

            var user = this.Session.Load<User>("Admin");
            this.Session.Delete(user);
            this.Session.SaveChanges();

            // expect base controller to redirect to /welcome
            return this.RedirectToAction("Index", "Home");
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // don't load the non-existing admin user
        }

        private void AssertAdminUserExists()
        {
            if (this.Session.Load<User>("Admin") != null)
            {
                this.Response.Redirect("/");
                this.Response.End();
            }
        }
    }
}