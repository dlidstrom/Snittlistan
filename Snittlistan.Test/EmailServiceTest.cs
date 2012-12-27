namespace Snittlistan.Test
{
    using System.Threading;

    using Snittlistan.Web.Services;

    using Xunit;

    public class EmailServiceTest
    {
        [Fact(Skip = "Configure before executing")]
        public void ShouldDeliverMail()
        {
            // this test actually sends a mail
            const string Host = "mail.snittlistan.se";
            const int Port = 1045;
            const string Username = "admin@snittlistan.se";
            const string Password = "____";
            var ev = new AutoResetEvent(false);
            var service = new EmailService(Host, Port, Username, Password, new[] { "dlidstrom@gmail.com" })
            {
                ResetEvent = ev
            };

            service.SendMail("dlidstrom@gmail.com", "Test", "Message body");
            Assert.True(ev.WaitOne(10000));
        }
    }
}