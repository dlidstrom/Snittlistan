#nullable enable

namespace Snittlistan.Web.Models
{
    public class InviteUserEmail : EmailBase
    {
        public InviteUserEmail(string recipient, string subject, string activationUri)
            : base("InviteUser", recipient, subject)
        {
            ActivationUri = activationUri;
        }

        public string ActivationUri { get; }
    }
}
