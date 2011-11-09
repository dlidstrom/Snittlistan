namespace Snittlistan.Test
{
    using Snittlistan.Services;
    using Xunit;

    public class EmailServiceTest
    {
        [Fact(Skip = "Configure before executing")]
        public void ShouldDeliverMail()
        {
            // this test actually sends a mail
            string host = "mail.snittlistan.se";
            int port = 1045;
            string username = "elmah@snittlistan.se";
            string password = "____";
            var service = new EmailService(host, port, username, password, new string[] { "dlidstrom@gmail.com" });

            service.SendMail("dlidstrom@gmail.com", "Test", "Message body");
        }
    }
}