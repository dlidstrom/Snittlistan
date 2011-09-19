using Moq;
using MvcContrib.TestHelper;
using Snittlistan.Events;
using Snittlistan.Handlers;
using Snittlistan.Models;
using Snittlistan.Services;
using Xunit;

namespace Snittlistan.Test
{
	public class SendRegistrationEmailHandlerTest
	{
		[Fact]
		public void ShouldSendMail()
		{
			// Arrange
			var service = Mock.Of<IEmailService>();
			var recipient = It.Is<string>(s => s == "e@d.com");
			var subject = It.Is<string>(s => s == "subject");
			bool mailSent = false;
			Mock.Get(service)
				.Setup(s => s.SendMail(recipient, subject, It.IsAny<string>()))
				.Callback(() => mailSent = true);

			// Act
			var handler = new SendRegistrationEmailHandler(service);
			handler.Handle(new NewUserCreatedEvent
			{
				User = new User("F", "L", "e@d.com", "some pwd")
			});

			// Assert
			mailSent.ShouldBe(true);
		}
	}
}
