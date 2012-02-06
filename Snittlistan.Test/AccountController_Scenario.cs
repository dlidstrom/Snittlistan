namespace Snittlistan.Test
{
    using System;
    using Moq;
    using MvcContrib.TestHelper;
    using Snittlistan.Controllers;
    using Snittlistan.Events;
    using Snittlistan.Helpers;
    using Snittlistan.Models;
    using Snittlistan.Services;
    using Snittlistan.ViewModels.Account;
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
            using (DomainEvent.Disable())
                controller1.Register(model);

            // normally done by infrastructure (special action filter)
            Session.SaveChanges();

            // let indexing do its job
            WaitForNonStaleResults<User>();

            // verify
            var registeredUser = Session.FindUserByEmail("e@d.com");
            registeredUser.ShouldNotBeNull("Should find user after registration");
            var key = registeredUser.ActivationKey;

            var controller2 = new AccountController(Session, Mock.Of<IAuthenticationService>());
            controller2.Verify(Guid.Parse(key));

            // let indexing do its job
            WaitForNonStaleResults<User>();

            // logon
            bool loggedOn = false;
            var service = Mock.Of<IAuthenticationService>();
            Mock.Get(service)
                .Setup(s => s.SetAuthCookie(It.Is<string>(e => e == "e@d.com"), It.IsAny<bool>()))
                .Callback(() => loggedOn = true);

            var controller3 = new AccountController(Session, service);
            controller3.Url = CreateUrlHelper();
            controller3.LogOn(new LogOnViewModel { Email = "e@d.com", Password = "some pwd" }, string.Empty);

            loggedOn.ShouldBe(true);
        }
    }
}