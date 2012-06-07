namespace Snittlistan.Test
{
    using System;
    using System.Web.Mvc;
    using Controllers;
    using Helpers;
    using Models;
    using Moq;
    using MvcContrib.TestHelper;
    using Services;
    using Xunit;

    public class AccountController_Verify : DbTest
    {
        [Fact]
        public void UnknownIdFails()
        {
            var controller = new AccountController(Session, Mock.Of<IAuthenticationService>());
            var result = controller.Verify(Guid.NewGuid());
            result.AssertActionRedirect().ToAction("Register");
        }

        [Fact]
        public void KnownUserIsActivatedAndShownSuccess()
        {
            var user = new User("F", "L", "e@d.com", "some pwd");
            Session.Store(user);
            Session.SaveChanges();

            VerifyActivationKeyForUser(user)
                .AssertActionRedirect()
                .ToAction("VerifySuccess");
            var storedUser = Session.FindUserByEmail("e@d.com");
            storedUser.ShouldNotBeNull("Failed to find user");
            storedUser.IsActive.ShouldBe(true);
        }

        [Fact]
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
            var controller = new AccountController(Session, service);
            var result = controller.Verify(Guid.Parse(user.ActivationKey));
            loggedSomebodyOn.ShouldBe(false);
            return result;
        }
    }
}
