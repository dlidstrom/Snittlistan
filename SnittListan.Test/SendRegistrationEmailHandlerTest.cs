using Moq;
using SnittListan.Services;
using Xunit;

namespace SnittListan.Test
{
	public class SendRegistrationEmailHandlerTest
	{
		[Fact]
		public void ShouldSendMail()
		{
			var handler = new SendRegistrationEmailHandler(Mock.Of<IEmailService>());
			Assert.True(false);
		}
	}
}
