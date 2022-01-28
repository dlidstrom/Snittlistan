#nullable enable

using Postal;

namespace Snittlistan.Web.Models;

public class OneTimePasswordEmail_State : EmailState
{
    public OneTimePasswordEmail_State(
        string to,
        string subject,
        string oneTimePassword)
        : base(OwnerEmail, to, OwnerEmail, subject)
    {
        OneTimePassword = oneTimePassword;
    }

    public string OneTimePassword { get; }

    public override Email CreateEmail()
    {
        return new OneTimePasswordEmail(
            To,
            Subject,
            OneTimePassword);
    }
}
