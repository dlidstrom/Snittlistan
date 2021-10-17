namespace Snittlistan.Queue.Messages
{
    using System;

    public class UserInvitedTask
    {
        public UserInvitedTask(string activationUri, string email)
        {
            ActivationUri = activationUri ?? throw new ArgumentNullException(nameof(activationUri));
            Email = email;
        }

        public string ActivationUri { get; }
        public string Email { get; }
    }
}