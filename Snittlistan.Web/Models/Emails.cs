namespace Snittlistan.Web.Models
{
    using System;
    using System.Configuration;
    using System.Text;
    using System.Web.Mvc;
    using JetBrains.Annotations;
    using Postal;
    using Snittlistan.Web.Areas.V2.ReadModels;

    public static class Emails
    {
        private static EmailService service;

        public static void SendOneTimePassword(
            string recipient,
            string subject,
            string oneTimePassword)
        {
            Send(
                "OneTimePassword",
                recipient,
                subject,
                o =>
                {
                    o.OneTimePassword = oneTimePassword;
                });
        }

        public static void InviteUser(string recipient, string subject, string activationUri)
        {
            Send(
                "InviteUser",
                recipient,
                subject,
                o =>
                {
                    o.ActivationUri = activationUri;
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

        public static void MatchRegistered(
            string team,
            string opponent,
            int score,
            int opponentScore,
            ResultSeriesReadModel resultSeriesReadModel,
            ResultHeaderReadModel resultHeaderReadModel)
        {
            var subject = $"{team} mot {opponent}: {score} - {opponentScore}";
            Send(
                "MatchRegistered",
                ConfigurationManager.AppSettings["OwnerEmail"],
                subject,
                o =>
                {
                    o.Subject = subject;
                    o.Team = team;
                    o.Opponent = opponent;
                    o.Score = score;
                    o.OpponentScore = opponentScore;
                    o.ResultSeriesReadModel = resultSeriesReadModel;
                    o.ResultHeaderReadModel = resultHeaderReadModel;
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

        public static void Initialize(string viewsPath)
        {
            var engines = new ViewEngineCollection
            {
                new FileSystemRazorViewEngine(viewsPath)
            };
            service = new EmailService(engines);
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
            email.Subject = $"=?UTF-8?B?{Convert.ToBase64String(Encoding.UTF8.GetBytes(subject))}?=";

            // add moderators
            var moderatorEmails = string.Join(", ", ConfigurationManager.AppSettings["OwnerEmail"].Split(';'));
            email.Bcc = moderatorEmails;
            action.Invoke(email);

            service.Send(email);
        }
    }
}