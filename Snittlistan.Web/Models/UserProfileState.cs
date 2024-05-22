#nullable enable


namespace Snittlistan.Web.Models;

public abstract class UserProfileState : EmailState
{
    public UserProfileState(
        string from,
        string to,
        string bcc,
        string subject,
        Uri userProfileLink)
        : base(from, to, bcc, subject)
    {
        UserProfileLink = userProfileLink;
    }

    public Uri UserProfileLink { get; }
}
