using Snittlistan.Web.DomainEvents;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.Handlers
{
    public class UserRegistered : IHandle<NewUserCreatedEvent>
    {
        public void Handle(NewUserCreatedEvent @event)
        {
            var recipient = @event.User.Email;
            const string Subject = "Välkommen till Snittlistan!";
            var activationKey = @event.User.ActivationKey;
            var id = @event.User.Id;

            Emails.UserRegistered(recipient, Subject, id, activationKey);
        }
    }
}