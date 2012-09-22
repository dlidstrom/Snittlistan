namespace Snittlistan.Test
{
    using System;

    using Moq;

    using Snittlistan.Web.Controllers;
    using Snittlistan.Web.Events;
    using Snittlistan.Web.Helpers;
    using Snittlistan.Web.Services;
    using Snittlistan.Web.ViewModels.Account;

    using Xunit;

    public class AccountController_Scenario : DbTest
    {
        [Fact]
        public void CanLogOnAfterRegisteringAndVerifyingAccount()
        {
            // register
            var model = new RegisterViewModel
                {
                    FirstName = "F",
                    LastName = "L",
                    Email = "e@d.com",
                    ConfirmEmail = "e@d.com",
                    Password = "some pwd"
                };

            var controller1 = new AccountController(Session, Mock.Of<IAuthenticationService>());
            using (DomainEvent.Disable()) controller1.Register(model);

            // normally done by infrastructure (special action filter)
            Session.SaveChanges();

            // verify
            var registeredUser = Session.FindUserByEmail("e@d.com");
            Assert.NotNull(registeredUser);
            var key = registeredUser.ActivationKey;

            var controller2 = new AccountController(Session, Mock.Of<IAuthenticationService>());
            controller2.Verify(Guid.Parse(key));

            // logon
            bool loggedOn = false;
            var service = Mock.Of<IAuthenticationService>();
            Mock.Get(service)
                .Setup(s => s.SetAuthCookie(It.Is<string>(e => e == "e@d.com"), It.IsAny<bool>()))
                .Callback(() => loggedOn = true);

            var controller3 = new AccountController(Session, service)
                {
                    Url = CreateUrlHelper()
                };
            controller3.LogOn(
                new LogOnViewModel
                    {
                        Email = "e@d.com",
                        Password = "some pwd"
                    },
                string.Empty);

            Assert.True(loggedOn);
        }
    }
}