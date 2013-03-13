using System;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.DomainEvents
{
    public class UserInvitedEvent : IDomainEvent
    {
        public UserInvitedEvent(User user)
        {
            if (user == null) throw new ArgumentNullException("user");
            this.User = user;
        }

        public User User { get; private set; }
    }
}