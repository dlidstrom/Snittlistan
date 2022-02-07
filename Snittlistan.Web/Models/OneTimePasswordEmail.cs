#nullable enable


namespace Snittlistan.Web.Models;

public class OneTimePasswordEmail : EmailBase
{
    private readonly OneTimePasswordEmail_State _state;

    public OneTimePasswordEmail(
        string to,
        string subject,
        string oneTimePassword)
        : base("OneTimePassword")
    {
        _state = new(
            to,
            subject,
            oneTimePassword);
    }

    public string OneTimePassword => _state.OneTimePassword;

    public override EmailState State => _state;
}
