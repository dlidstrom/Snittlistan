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
			string body = "Du får det här brevet för att du har försökt registrera dig på Snittlistan.se."
				+ "\n"
				+ string.Format("Klicka här för att aktivera ditt konto: http://www.snittlistan.se/account/validate?id={0}", @event.User.ActivationKey);

			emailService.SendMail(recipient, subject, body);
		}
	}
}
