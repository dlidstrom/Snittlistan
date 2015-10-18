using System;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.DomainEvents
{
    public class UserInvitedEvent : IDomainEvent
    {
        public UserInvitedEvent(User user, string activationUri)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (activationUri == null) throw new ArgumentNullException("activationUri");
            User = user;
            ActivationUri = activationUri;
        }

        public User User { get; private set; }
        public string ActivationUri { get; private set; }
    }
}