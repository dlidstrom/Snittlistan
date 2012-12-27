namespace Snittlistan.Test
{
    using System;

    using Castle.Windsor;

    using Moq;

    using Snittlistan.Web.Areas.V1.Controllers;
    using Snittlistan.Web.Areas.V1.ViewModels.Account;
    using Snittlistan.Web.Events;
    using Snittlistan.Web.Helpers;
    using Snittlistan.Web.Services;

    using Xunit;

    public class AccountController_Scenario : DbTest
    {
        private readonly IWindsorContainer oldContainer;

        public AccountController_Scenario()
        {
            this.oldContainer = DomainEvent.SetContainer(new WindsorContainer());
        }

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
            var loggedOn = false;
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

        protected override void OnDispose()
        {
            DomainEvent.SetContainer(oldContainer);
        }
    }
}