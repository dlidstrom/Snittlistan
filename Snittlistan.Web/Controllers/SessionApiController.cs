namespace Snittlistan.Web.Controllers
{
    using System.Net;
    using System.Web.Mvc;

    using Raven.Client;

    using Snittlistan.Web.Helpers;
    using Snittlistan.Web.Services;

    public class SessionApiController : AbstractController
    {
        private readonly IAuthenticationService authenticationService;

        public SessionApiController(IDocumentSession session, IAuthenticationService authenticationService)
            : base(session)
        {
            this.authenticationService = authenticationService;
        }

        [HttpPost]
        [ActionName("Session")]
        public ActionResult CreateSession(string email, string password, string remember)
        {
            InputError error = null;
            var user = this.Session.FindUserByEmail(email);
            if (user == null)
                error = new InputError("email", "Användaren existerar inte");
            else if (!user.ValidatePassword(password))
                error = new InputError("password", "Lösenordet stämmer inte!");
            else if (user.IsActive == false)
                error = new InputError("Inactive", "Användaren har inte aktiverats");

            // any error?
            if (error != null)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.Json(new { error });
            }

            // sign in user by creating authentication cookie
            this.authenticationService.SetAuthCookie(email, remember == "on");
            return this.Json(new { isAuthenticated = true, email });
        }

        [HttpDelete]
        [ActionName("Session")]
        public ActionResult DeleteSession()
        {
            this.authenticationService.SignOut();
            return this.Json(new { isAuthenticated = false });
        }
    }
}
