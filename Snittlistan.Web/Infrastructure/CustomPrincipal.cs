namespace Snittlistan.Web.Infrastructure
{
    using System.Linq;
    using System.Security.Principal;

    public class CustomPrincipal : IPrincipal
    {
        private readonly string[] roles;

        public CustomPrincipal(string playerId, string name, string[] roles, string email)
        {
            Email = email;
            this.roles = roles;
            CustomIdentity = new CustomIdentity(playerId, name);
        }

        public string Email { get; }

        public bool IsInRole(string role)
        {
            return roles.Contains(role);
        }

        public IIdentity Identity => CustomIdentity;

        public CustomIdentity CustomIdentity { get; }
    }
}