using System.Configuration;
using System.Linq;
using System.Net.Mail;

namespace SnittListan.Services
{
	public class EmailService : IEmailService
	{
		public void SendMail(string recipient, string subject, string body)
		{
			var mailMessage = new MailMessage
			{
				Body = body,
				Subject = subject
			};
			mailMessage.To.Add(new MailAddress(recipient));

			// add moderator
			var moderatorEmails = ConfigurationManager.AppSettings["OwnerEmail"];
			moderatorEmails
				.Split(';')
				.Select(e => new MailAddress(e.Trim()))
				.ToList()
				.ForEach(m => mailMessage.To.Add(m));

			using (var smtpClient = new SmtpClient())
			{
				smtpClient.Send(mailMessage);
			}
		}
	}
}
