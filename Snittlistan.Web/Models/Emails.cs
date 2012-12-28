namespace Snittlistan.Web.Models
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Configuration;
    using System.Net.Mail;
    using System.Text;
    using System.Web;
    using System.Web.Configuration;

    using JetBrains.Annotations;

    using Postal;

    public static class Emails
    {
        public static void InviteUser(string recipient, string subject, string id, string activationKey)
        {
            Send(
                "InviteUser",
                recipient,
                subject,
                o =>
                {
                    o.Id = id;
                    o.ActivationKey = activationKey;
                });
        }

        public static void UserRegistered(string recipient, string subject, string id, string activationKey)
        {
            Send(
                "UserRegistered",
                recipient,
                subject,
                o =>
                    {
                        o.Id = id;
                        o.ActivationKey = activationKey;
                    });
        }

        private static void Send(
            [AspMvcView] string view,
            string recipient,
            string subject,
            Action<dynamic> action)
        {
            dynamic email = new Email(view);
            email.To = recipient;
            email.From = ConfigurationManager.AppSettings["OwnerEmail"];
            email.Subject = string.Format("=?utf-8?B?{0}?=", Convert.ToBase64String(Encoding.UTF8.GetBytes(subject)));
            // add moderators
            var moderators = new MailAddressCollection();
            // fetch settings from web.config
            var config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            var settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
            Debug.Assert(settings != null, "settings != null");
            var moderatorEmails = ConfigurationManager.AppSettings["OwnerEmail"].Split(';')
                .Select(e => new MailAddress(e.Trim()))
                .ToList();
            moderatorEmails.ForEach(moderators.Add);
            email.Bcc = moderators;
            action.Invoke(email);
            email.Send();
        }
    }
}