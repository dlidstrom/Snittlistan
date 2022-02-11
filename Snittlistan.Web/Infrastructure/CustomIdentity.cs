
using System.Security.Principal;

#nullable enable

namespace Snittlistan.Web.Infrastructure;
public class CustomIdentity : GenericIdentity
{
    public CustomIdentity(string? playerId, string name, string email, string uniqueId)
        : base(name)
    {
        PlayerId = playerId;
        Email = email;
        UniqueId = uniqueId;
    }

    public string Email { get; }

    public string UniqueId { get; }

    public string? PlayerId { get; }
}
