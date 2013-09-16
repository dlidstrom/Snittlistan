using System.Security.Principal;
using System.Web;
using Raven.Client;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.HtmlHelpers
{
    public static class RequestExtensions
    {
        public static bool IsAdmin(this HttpRequestBase request, IPrincipal principal)
        {
            var session = MvcApplication.Container.Resolve<IDocumentSession>();
            var admin = session.Load<User>("Admin");
            return admin != null && request.IsAuthenticated && principal.Identity.Name == admin.Email;
        }
    }
}