#nullable enable

using System.Security.Principal;

namespace Snittlistan.Web.Infrastructure;

public class CustomPrincipal : IPrincipal
{
    private readonly string[] roles;

    public CustomPrincipal(string? playerId, string name, string email, string uniqueId, string[] roles)
    {
        this.roles = roles;
        CustomIdentity = new CustomIdentity(playerId, name, email, uniqueId);
    }

    public bool IsInRole(string role)
    {
        return roles.Contains(role);
    }

    public IIdentity Identity => CustomIdentity;

    public CustomIdentity CustomIdentity { get; }
}
