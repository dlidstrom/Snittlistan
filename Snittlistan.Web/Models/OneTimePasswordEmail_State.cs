#nullable enable

using Postal;

namespace Snittlistan.Web.Models;

public class OneTimePasswordEmail_State : EmailState
{
    public OneTimePasswordEmail_State(
        string to,
        string subject,
        string oneTimePassword,
        Uri userProfileLink)
        : base(OwnerEmail, to, BccEmail, subject, userProfileLink)
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
