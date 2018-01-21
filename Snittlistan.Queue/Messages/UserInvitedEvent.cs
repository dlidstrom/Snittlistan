using System;

namespace Snittlistan.Queue.Messages
{
    public class UserInvitedEvent
    {
        public UserInvitedEvent(string activationUri, string email)
        {
            ActivationUri = activationUri ?? throw new ArgumentNullException(nameof(activationUri));
            Email = email;
        }

        public string ActivationUri { get; }
        public string Email { get; }
    }
}