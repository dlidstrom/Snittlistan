using Moq;
using Snittlistan.Web.Areas.V1.Controllers;
using Snittlistan.Web.Services;
using Xunit;

namespace Snittlistan.Test.Controllers
{
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

            var controller = new AccountController(service) { DocumentSession = Session };
            var result = controller.LogOff();
            result.AssertActionRedirect().ToController("Home").ToAction("Index");
            Assert.True(signedOut);
        }
    }
}