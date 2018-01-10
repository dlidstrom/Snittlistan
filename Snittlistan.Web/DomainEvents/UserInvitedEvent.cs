using System;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.DomainEvents
{
    public class UserInvitedEvent : IDomainEvent
    {
        public UserInvitedEvent(User user, string activationUri)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
            ActivationUri = activationUri ?? throw new ArgumentNullException(nameof(activationUri));
        }

        public User User { get; private set; }
        public string ActivationUri { get; private set; }
    }
}