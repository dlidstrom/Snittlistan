namespace Snittlistan.Services
{
    /// <summary>
    /// Represents a mail service.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Send a mail to a recipient.
        /// </summary>
        /// <param name="recipient">Recipient address.</param>
        /// <param name="subject">Mail subject.</param>
        /// <param name="body">Mail body.</param>
        void SendMail(string recipient, string subject, string body);
    }
}