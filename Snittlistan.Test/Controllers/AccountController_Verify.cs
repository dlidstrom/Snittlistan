using System;
using System.Web.Mvc;
using Castle.Windsor;
using Moq;
using Snittlistan.Web.Areas.V1.Controllers;
using Snittlistan.Web.DomainEvents;
using Snittlistan.Web.Helpers;
using Snittlistan.Web.Models;
using Snittlistan.Web.Services;
using Xunit;

namespace Snittlistan.Test.Controllers
{
    public class AccountController_Verify : DbTest
    {
        private readonly IWindsorContainer oldContainer;

        public AccountController_Verify()
        {
            oldContainer = DomainEvent.SetContainer(new WindsorContainer());
        }

        [Fact]
        public void UnknownIdFails()
        {
            var controller = new AccountController(Mock.Of<IAuthenticationService>()) { DocumentSession = Session };
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
            Assert.NotNull(storedUser);
            Assert.True(storedUser.IsActive);
        }

        [Fact]
        public void ActivatedUserRedirectsToLogOn()
        {
            var user = CreateActivatedUser("F", "L", "e@d.com", "some pwd");

            VerifyActivationKeyForUser(user)
                .AssertActionRedirect()
                .ToAction("LogOn");
        }

        protected override void OnTearDown()
        {
            DomainEvent.SetContainer(oldContainer);
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