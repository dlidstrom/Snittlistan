using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using SnittListan.Handlers;
using SnittListan.Services;
using Xunit;

namespace SnittListan.Test
{
	public class EmailServiceStub : IEmailService
	{
		public List<string> EmailsToSend { get; set; }
		public void SendMail(string recipient, string subject, string body)
		{
			EmailsToSend.Add(recipient + ": " + subject);
		}
	}

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
	}
}
