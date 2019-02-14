namespace Snittlistan.Web.Infrastructure
{
    using System.Linq;
    using System.Security.Principal;
    using System.Web;

    public class CustomPrincipal : IPrincipal
    {
        private readonly string[] roles;

        public CustomPrincipal(string playerId, string name, string[] roles)
        {
            this.roles = roles;
            CustomIdentity = new CustomIdentity(playerId, name);
        }

        public bool IsInRole(string role)
        {
            return HttpContext.Current.Request.IsAuthenticated && roles.Contains(role);
        }

        public IIdentity Identity => CustomIdentity;

        public CustomIdentity CustomIdentity { get; }
    }
}