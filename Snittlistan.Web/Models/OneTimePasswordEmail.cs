#nullable enable

namespace Snittlistan.Web.Models
{
    public class OneTimePasswordEmail : EmailBase
    {
        public OneTimePasswordEmail(
            string to,
            string subject,
            string oneTimePassword)
            : base("OneTimePassword", to, subject)
        {
            OneTimePassword = oneTimePassword;
        }

        public string OneTimePassword { get; }
    }
}
