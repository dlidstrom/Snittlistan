using System;
using System.Configuration;
using System.Threading;
using System.Web.Mvc;
using NLog;
using Snittlistan.Web.Areas.V1.ViewModels.Account;
using Snittlistan.Web.Controllers;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.Areas.V1.Controllers
{
    public class WelcomeController : AbstractController
    {
        private const string MaintenanceAuthenticationTokenConstant = "Maintenance Authentication Token";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

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
            user.Activate(false);
            this.DocumentSession.Store(user);

            return this.RedirectToAction("Success");
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

            var user = this.DocumentSession.Load<User>("Admin");
            this.DocumentSession.Delete(user);
            this.DocumentSession.SaveChanges();

            // expect base controller to redirect to /welcome
            return this.RedirectToAction("Index", "Home");
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // don't load the non-existing admin user
        }

        private void AssertAdminUserExists()
        {
            if (this.DocumentSession.Load<User>("Admin") != null)
            {
                this.Response.Redirect("/");
                this.Response.End();
            }
        }
    }
}