using Moq;
using NUnit.Framework;
using Snittlistan.Web.Areas.V1.Controllers;
using Snittlistan.Web.Services;

namespace Snittlistan.Test.Controllers
{
    [TestFixture]
    public class AccountController_LogOff : DbTest
    {
        [Test]
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