namespace Snittlistan.Web.Infrastructure
{
    using System.Security.Principal;

    public class CustomIdentity : GenericIdentity
    {
        public CustomIdentity(string playerId, string name) : base(name)
        {
            PlayerId = playerId;
        }

        // nullable => User
        public string PlayerId { get; }
    }
}