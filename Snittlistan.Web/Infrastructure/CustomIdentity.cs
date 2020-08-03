namespace Snittlistan.Web.Infrastructure
{
    using System.Security.Principal;

    public class CustomIdentity : GenericIdentity
    {
        public CustomIdentity(string playerId, string name, string email, string uniqueId)
            : base(name)
        {
            PlayerId = playerId;
            Email = email;
            UniqueId = uniqueId;
        }

        public string Email { get; }

        public string UniqueId { get; }

        // nullable => User
        public string PlayerId { get; }
    }
}