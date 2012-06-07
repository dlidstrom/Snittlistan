namespace Snittlistan.Test
{
    using Controllers;
    using Moq;
    using MvcContrib.TestHelper;
    using Services;
    using Xunit;

    public class AccountController_LogOff : DbTest
    {
        [Fact]
        public void ShouldSignOut()
        {
            var service = Mock.Of<IAuthenticationService>();
            bool signedOut = false;
            Mock.Get(service)
                .Setup(s => s.SignOut())
                .Callback(() => signedOut = true);

            var controller = new AccountController(Session, service);
            var result = controller.LogOff();
            result.AssertActionRedirect().ToController("Home").ToAction("Index");
            signedOut.ShouldBe(true);
        }
    }
}