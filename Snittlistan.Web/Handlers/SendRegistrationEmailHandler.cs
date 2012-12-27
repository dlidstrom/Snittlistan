namespace Snittlistan.Web.Handlers
{
    using Snittlistan.Web.Areas.V1.ViewModels.Account;
    using Snittlistan.Web.Events;
    using Snittlistan.Web.Helpers;
    using Snittlistan.Web.Services;

    public class SendRegistrationEmailHandler : IHandle<NewUserCreatedEvent>
    {
        private readonly IEmailService emailService;

        public SendRegistrationEmailHandler(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        public void Handle(NewUserCreatedEvent @event)
        {
            var recipient = @event.User.Email;
            const string Subject = "Välkommen till Snittlistan!";

            var viewModel = new RegistrationMailViewModel {
                                                              ActivationKey = @event.User.ActivationKey
                                                          };
            var body = ViewHelper.RenderEmailBody(viewModel, "Registration");

            this.emailService.SendMail(recipient, Subject, body);
        }
    }
}