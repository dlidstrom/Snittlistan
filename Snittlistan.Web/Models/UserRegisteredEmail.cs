#nullable enable

namespace Snittlistan.Web.Models
{
    public class UserRegisteredEmail : EmailBase
    {
        public UserRegisteredEmail(
            string recipient,
            string subject,
            string id,
            string activationKey)
            : base("UserRegistered", recipient, subject)
        {
            Id = id;
            ActivationKey = activationKey;
        }

        public string Id { get; }
        public string ActivationKey { get; }
    }
}
