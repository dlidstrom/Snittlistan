#nullable enable

namespace Snittlistan.Web.Models
{
    public class OneTimePasswordEmail : EmailBase
    {
        public OneTimePasswordEmail(
            string recipient,
            string subject,
            string oneTimePassword)
            : base("OneTimePassword", recipient, subject)
        {
            OneTimePassword = oneTimePassword;
        }

        public string OneTimePassword { get; }
    }
}
