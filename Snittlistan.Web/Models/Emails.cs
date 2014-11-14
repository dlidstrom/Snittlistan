using System;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using JetBrains.Annotations;
using Postal;

namespace Snittlistan.Web.Models
{
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

        public static void MatchRegistered(string subject, string team, string opponent, int score, int awayScore)
        {
            Send(
                "MatchRegistered",
                ConfigurationManager.AppSettings["OwnerEmail"],
                subject,
                o =>
                {
                    o.Team = team;
                    o.Opponent = opponent;
                    o.Score = score;
                    o.AwayScore = awayScore;
                });
        }

        public static void SendMail(string email, string subject, string content)
        {
            Send(
                "Mail",
                email,
                subject,
                o => o.Content = content);
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