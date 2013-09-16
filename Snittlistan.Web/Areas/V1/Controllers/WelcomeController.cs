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
            user.Activate(false);
            DocumentSession.Store(user);

            return RedirectToAction("Success");
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Reset()
        {
            Log.Info("Maintenance Authentication Token: {0}", MaintenanceAuthenticationToken);
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

            var user = DocumentSession.Load<User>("Admin");
            DocumentSession.Delete(user);
            DocumentSession.SaveChanges();

            // expect base controller to redirect to /welcome
            return RedirectToAction("Index", "Home");
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // don't load the non-existing admin user
        }

        private void AssertAdminUserExists()
        {
            if (DocumentSession.Load<User>("Admin") == null) return;
            Response.Redirect("/");
            Response.End();
        }
    }
}