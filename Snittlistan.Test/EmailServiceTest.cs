namespace Snittlistan.Test
{
    using System.Threading;
    using MvcContrib.TestHelper;
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
            var ev = new AutoResetEvent(false);
            var service = new EmailService(host, port, username, password, new string[] { "dlidstrom@gmail.com" })
            {
                ResetEvent = ev
            };

            service.SendMail("dlidstrom@gmail.com", "Test", "Message body");
            ev.WaitOne(10000).ShouldBe(true);
        }
    }
}