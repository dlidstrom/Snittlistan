using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Snittlistan.Handlers;
using Snittlistan.Services;
using Xunit;

namespace Snittlistan.Test
{
	public class SendRegistrationEmailHandlerTest
	{
		[Fact(Skip = "Not sure how to assert")]
		public void ShouldSendMail()
		{
			var es = new EmailServiceStub();
			var handler = new SendRegistrationEmailHandler(es);
			var container = new WindsorContainer()
				.Register(Component.For<IEmailService>().ImplementedBy<EmailServiceStub>());
		}

		public class EmailServiceStub : IEmailService
		{
			public List<string> EmailsToSend { get; set; }
			public void SendMail(string recipient, string subject, string body)
			{
				EmailsToSend.Add(recipient + ": " + subject);
			}
		}
	}
}
