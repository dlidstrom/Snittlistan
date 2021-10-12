#nullable enable

namespace Snittlistan.Web.Models
{
    using System;
    using System.Configuration;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Postal;
    using Snittlistan.Web.Areas.V2.Domain;
    using Snittlistan.Web.Areas.V2.ReadModels;

    public static class Emails
    {
        public class UpdateMailViewModel : Email
        {
            public UpdateMailViewModel(
                string to,
                FormattedAuditLog formattedAuditLog,
                string[] players,
                string? teamLeader)
                : base("UpdateMail")
            {
                To = to;
                FormattedAuditLog = formattedAuditLog;
                Players = players;
                TeamLeader = teamLeader;
            }

            public string Bcc { get; } = ConfigurationManager.AppSettings["OwnerEmail"];
            public string From { get; } = ConfigurationManager.AppSettings["OwnerEmail"];
            public string Subject { get; } = "Uttagning har uppdaterats";
            public string To { get; }
            public FormattedAuditLog FormattedAuditLog { get; }
            public string[] Players { get; }
            public string? TeamLeader { get; }
        }

        public static async Task SendOneTimePassword(
            IEmailService service,
            string recipient,
            string subject,
            string oneTimePassword)
        {
            await Send(
                service,
                "OneTimePassword",
                recipient,
                subject,
                o =>
                {
                    o.OneTimePassword = oneTimePassword;
                });
        }

        public static async Task InviteUser(
            IEmailService service,
            string recipient,
            string subject,
            string activationUri)
        {
            await Send(
                service,
                "InviteUser",
                recipient,
                subject,
                o =>
                {
                    o.ActivationUri = activationUri;
                });
        }

        public static async Task UserRegistered(
            IEmailService service,
            string recipient,
            string subject,
            string id,
            string activationKey)
        {
            await Send(
                service,
                "UserRegistered",
                recipient,
                subject,
                o =>
                {
                    o.Id = id;
                    o.ActivationKey = activationKey;
                });
        }

        public static async Task MatchRegistered(
            IEmailService service,
            string team,
            string opponent,
            int score,
            int opponentScore,
            ResultSeriesReadModel resultSeriesReadModel,
            ResultHeaderReadModel resultHeaderReadModel)
        {
            string subject = $"{team} mot {opponent}: {score} - {opponentScore}";
            await Send(
                service,
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

        public static async Task SendAdminMail(
            IEmailService service,
            string subject,
            string content)
        {
            await Send(
                service,
                "Mail",
                ConfigurationManager.AppSettings["OwnerEmail"],
                subject,
                o => o.Content = content);
        }

        public static async Task SendMail(
            IEmailService service,
            string email,
            string subject,
            string content)
        {
            await Send(
                service,
                "Mail",
                email,
                subject,
                o => o.Content = content);
        }

        private static async Task Send(
            IEmailService service,
            string view,
            string recipient,
            string subject,
            Action<dynamic> action)
        {
            dynamic email = new Email(view);
            email.To = recipient;
            email.From = ConfigurationManager.AppSettings["OwnerEmail"];
            email.Subject = subject;

            // add moderators
            string moderatorEmails = string.Join(", ", ConfigurationManager.AppSettings["OwnerEmail"].Split(';'));
            email.Bcc = moderatorEmails;
            action.Invoke(email);

            await service.SendAsync(email);
        }
    }
}
