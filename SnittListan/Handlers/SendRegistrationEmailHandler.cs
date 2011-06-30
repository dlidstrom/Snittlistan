using SnittListan.Events;
using SnittListan.Handlers;
using SnittListan.Services;

namespace SnittListan.Handlers
{
	public class SendRegistrationEmailHandler : IHandle<NewUserCreatedEvent>
	{
		private IEmailService emailService;

		public SendRegistrationEmailHandler(IEmailService emailService)
		{
			this.emailService = emailService;
		}

		public void Handle(NewUserCreatedEvent @event)
		{
			string recipient = @event.User.Email;
			string subject = "Välkommen till SnittListan!";
			string body = string.Empty;

			emailService.SendMail(recipient, subject, body);
		}
	}
}
