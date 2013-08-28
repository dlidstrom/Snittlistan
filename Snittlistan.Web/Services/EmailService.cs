using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Elmah;

namespace Snittlistan.Web.Services
{
    /// <summary>
    /// Used to send email.
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly string host;
        private readonly int port;
        private readonly string username;
        private readonly string password;
        private readonly MailAddress from;
        private readonly List<MailAddress> moderatorEmails;

        /// <summary>
        /// Initializes a new instance of the EmailService class.
        /// </summary>
        public EmailService()
        {
            // fetch settings from web.config
            var config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            var settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
            Debug.Assert(settings != null, "settings != null");
            host = settings.Smtp.Network.Host;
            port = settings.Smtp.Network.Port;
            username = settings.Smtp.Network.UserName;
            password = settings.Smtp.Network.Password;
            from = new MailAddress(username);
            moderatorEmails = ConfigurationManager
                .AppSettings["OwnerEmail"]
                .Split(';')
                .Select(e => new MailAddress(e.Trim()))
                .ToList();
        }

        /// <summary>
        /// Initializes a new instance of the EmailService class.
        /// </summary>
        /// <param name="host">Smtp host address.</param>
        /// <param name="port">Smtp host port.</param>
        /// <param name="username">User name.</param>
        /// <param name="password">User password.</param>
        /// <param name="moderators">List of moderator addresses.</param>
        public EmailService(string host, int port, string username, string password, IEnumerable<string> moderators)
        {
            this.host = host;
            this.port = port;
            this.username = username;
            this.password = password;
            from = new MailAddress(username);
            moderatorEmails = moderators.Select(e => new MailAddress(e.Trim())).ToList();
        }

        /// <summary>
        /// Gets or sets the reset event. Use to wait for email to be sent.
        /// </summary>
        public AutoResetEvent ResetEvent
        {
            get;
            set;
        }

        /// <summary>
        /// Send mail to a recipient and the moderators.
        /// </summary>
        /// <param name="recipient">Mail recipient.</param>
        /// <param name="subject">Mail subject.</param>
        /// <param name="body">Mail body (as html).</param>
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
                From = from,
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
                smtpClient.Credentials = new NetworkCredential
                {
                    UserName = username,
                    Password = password
                };
                smtpClient.Send(mailMessage);
            }

            if (ResetEvent != null)
                ResetEvent.Set();
        }
    }
}