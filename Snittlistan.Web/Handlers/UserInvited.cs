namespace Snittlistan.Web.Handlers
{
    using JetBrains.Annotations;

    using Snittlistan.Web.Events;
    using Snittlistan.Web.Models;

    [UsedImplicitly]
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