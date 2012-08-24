namespace Snittlistan.Controllers
{
    using System.Net;
    using System.Web.Mvc;
    using Helpers;
    using Raven.Client;
    using Services;

    public class ApiController : AbstractController
    {
        private readonly IAuthenticationService authenticationService;

        public ApiController(IDocumentSession session, IAuthenticationService authenticationService)
            : base(session)
        {
            this.authenticationService = authenticationService;
        }

        [HttpPost]
        [ActionName("Session")]
        public ActionResult CreateSession(string email, string password, string remember)
        {
            InputError error = null;
            var user = Session.FindUserByEmail(email);
            if (user == null)
                error = new InputError("email", "Användaren existerar inte");
            else if (!user.ValidatePassword(password))
                error = new InputError("password", "Lösenordet stämmer inte!");
            else if (user.IsActive == false)
                error = new InputError("Inactive", "Användaren har inte aktiverats");

            // any error?
            if (error != null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { error });
            }

            // sign in user by creating authentication cookie
            authenticationService.SetAuthCookie(email, remember == "on");
            return Json(new { isAuthenticated = true, email });
        }

        [HttpDelete]
        [ActionName("Session")]
        public ActionResult DeleteSession()
        {
            authenticationService.SignOut();
            return Json(new { isAuthenticated = false });
        }
    }
}
