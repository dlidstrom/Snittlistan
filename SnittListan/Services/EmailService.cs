using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Elmah;

namespace SnittListan.Services
{
	public class EmailService : IEmailService
	{
		private string host;
		private int port;
		private string username;
		private string password;
		private MailAddress from;
		private List<MailAddress> moderatorEmails;

		public EmailService()
		{
			// fetch settings from web.config
			var config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
			var settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
			this.host = settings.Smtp.Network.Host;
			this.port = settings.Smtp.Network.Port;
			this.username = settings.Smtp.Network.UserName;
			this.password = settings.Smtp.Network.Password;
			this.from = new MailAddress(this.username);
			this.moderatorEmails = ConfigurationManager
				.AppSettings["OwnerEmail"]
				.Split(';')
				.Select(e => new MailAddress(e.Trim()))
				.ToList();
		}

		public EmailService(string host, int port, string username, string password, IEnumerable<string> moderators)
		{
			this.host = host;
			this.port = port;
			this.username = username;
			this.password = password;
			this.from = new MailAddress(username);
			this.moderatorEmails = moderators.Select(e => new MailAddress(e.Trim())).ToList();
		}

		public void SendMail(string recipient, string subject, string body)
		{
			// execute asynchronously
			Task.Factory.StartNew(
				() => DoWork(recipient, subject, body),
				TaskCreationOptions.LongRunning)
					.ContinueWith(
						task => ErrorLog.GetDefault(null).Log(new Error(task.Exception)),
						TaskContinuationOptions.OnlyOnFaulted);
		}

		private void DoWork(string recipient, string subject, string body)
		{
			var mailMessage = new MailMessage
			{
				Body = body,
				Subject = subject,
				From = this.from,
				IsBodyHtml = true
			};
			mailMessage.To.Add(new MailAddress(recipient));

			// add moderators
			moderatorEmails.ForEach(m => mailMessage.Bcc.Add(m));

			using (var smtpClient = new SmtpClient
			{
				Host = host,
				Port = port,
				UseDefaultCredentials = false
			})
			{
				smtpClient.Credentials = new NetworkCredential { UserName = this.username, Password = this.password };
				smtpClient.Send(mailMessage);
			}
		}
	}
}
