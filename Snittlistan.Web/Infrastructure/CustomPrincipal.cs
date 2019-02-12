namespace Snittlistan.Web.Infrastructure
{
    using System.Linq;
    using System.Security.Principal;

    public class CustomPrincipal : IPrincipal
    {
        private readonly string[] roles;

        public CustomPrincipal(string name, string[] roles)
        {
            this.roles = roles;
            Identity = new GenericIdentity(name);
        }

        public bool IsInRole(string role)
        {
            return roles.Contains(role);
        }

        public IIdentity Identity { get; }
    }
}