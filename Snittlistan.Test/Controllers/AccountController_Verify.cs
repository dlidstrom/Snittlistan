using System;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Snittlistan.Web.Areas.V1.Controllers;
using Snittlistan.Web.Helpers;
using Snittlistan.Web.Models;
using Snittlistan.Web.Services;

namespace Snittlistan.Test.Controllers
{
    [TestFixture]
    public class AccountController_Verify : DbTest
    {
        [Test]
        public void UnknownIdFails()
        {
            var controller = new AccountController(Mock.Of<IAuthenticationService>()) { DocumentSession = Session };
            var result = controller.Verify(Guid.NewGuid());
            result.AssertActionRedirect().ToAction("Register");
        }

        [Test]
        public void KnownUserIsActivatedAndShownSuccess()
        {
            var user = new User("F", "L", "e@d.com", "some pwd");
            Session.Store(user);
            Session.SaveChanges();

            VerifyActivationKeyForUser(user)
                .AssertActionRedirect()
                .ToAction("VerifySuccess");
            var storedUser = Session.FindUserByEmail("e@d.com");
            Assert.NotNull(storedUser);
            Assert.True(storedUser.IsActive);
        }

        [Test]
        public void ActivatedUserRedirectsToLogOn()
        {
            var user = CreateActivatedUser("F", "L", "e@d.com", "some pwd");

            VerifyActivationKeyForUser(user)
                .AssertActionRedirect()
                .ToAction("LogOn");
        }

        private ActionResult VerifyActivationKeyForUser(User user)
        {
            bool loggedSomebodyOn = false;
            var service = Mock.Of<IAuthenticationService>();
            Mock.Get(service)
                .Setup(s => s.SetAuthCookie(It.IsAny<string>(), It.IsAny<bool>()))
                .Callback(() => loggedSomebodyOn = true);
            var controller = new AccountController(service) { DocumentSession = Session };
            var result = controller.Verify(Guid.Parse(user.ActivationKey));
            Assert.False(loggedSomebodyOn);
            return result;
        }
    }
}