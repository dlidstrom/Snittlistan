using Snittlistan.Web.DomainEvents;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.Handlers
{
    public class UserInvited : IHandle<UserInvitedEvent>
    {
        public void Handle(UserInvitedEvent @event)
        {
            var recipient = @event.User.Email;
            const string Subject = "Välkommen till Snittlistan!";
            var activationKey = @event.User.ActivationKey;
            var id = @event.User.Id;

            Emails.InviteUser(recipient, Subject, id, activationKey);
        }
    }
}