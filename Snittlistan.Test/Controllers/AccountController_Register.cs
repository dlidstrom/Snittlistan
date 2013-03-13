using Snittlistan.Web.DomainEvents;

namespace Snittlistan.Test
{
    using System;
    using System.Web.Mvc;

    using Castle.Windsor;

    using Moq;

    using Snittlistan.Web.Areas.V1.Controllers;
    using Snittlistan.Web.Areas.V1.ViewModels.Account;
    using Snittlistan.Web.Helpers;
    using Snittlistan.Web.Services;

    using Xunit;

    public class AccountController_Register : DbTest
    {
        private readonly IWindsorContainer oldContainer;

        public AccountController_Register()
        {
            this.oldContainer = DomainEvent.SetContainer(new WindsorContainer());
        }

        protected override void OnDispose()
        {
            DomainEvent.SetContainer(oldContainer);
        }

        [Fact]
        public void ShouldInitializeNewUser()
        {
            NewUserCreatedEvent ev = null;
            using (DomainEvent.TestWith(e => ev = (NewUserCreatedEvent)e))
            {
                var controller = new AccountController(Mock.Of<IAuthenticationService>()) { DocumentSession = Session };
                controller.Register(new RegisterViewModel
                {
                    FirstName = "first name",
                    LastName = "last name",
                    Email = "email",
                });
            }

            Assert.NotNull(ev);
        }

        [Fact]
        public void ShouldCreateInitializedUser()
        {
            var controller = new AccountController(Mock.Of<IAuthenticationService>()) { DocumentSession = Session };
            using (DomainEvent.Disable())
                controller.Register(new RegisterViewModel
                {
                    FirstName = "first name",
                    LastName = "last name",
                    Email = "email",
                });

            // normally done by infrastructure (special action filter)
            Session.SaveChanges();

            var user = Session.FindUserByEmail("email");
            Assert.NotNull(user);
            Assert.Equal("first name", user.FirstName);
            Assert.Equal("last name", user.LastName);
            Assert.Equal("email", user.Email);
            Assert.False(user.IsActive);
        }

        [Fact]
        public void RegisterDoesNotLogin()
        {
            using (DomainEvent.Disable())
            {
                var authService = Mock.Of<IAuthenticationService>();

                // assert through mock object
                Mock.Get(authService)
                    .Setup(s => s.SetAuthCookie(It.IsAny<string>(), It.IsAny<bool>()))
                    .Throws(new Exception("Register should not set authorization cookie"));
                var controller = new AccountController(authService) { DocumentSession = Session };
                controller.Register(new RegisterViewModel
                    {
                        FirstName = "f",
                        LastName = "l",
                        Email = "email"
                    });
            }
        }

        [Fact]
        public void SuccessfulRegisterRedirectsToSuccessPage()
        {
            using (DomainEvent.Disable())
            {
                var controller = new AccountController(Mock.Of<IAuthenticationService>()) { DocumentSession = Session };
                var result = controller.Register(new RegisterViewModel
                {
                    FirstName = "f",
                    LastName = "l",
                    Email = "email"
                });

                result.AssertActionRedirect().ToAction("RegisterSuccess");
            }
        }

        [Fact]
        public void CannotRegisterSameEmailTwice()
        {
            // Arrange
            CreateActivatedUser("F", "L", "e@d.com", "some pwd");
            var controller = new AccountController(Mock.Of<IAuthenticationService>()) { DocumentSession = Session };

            // Act
            var result = controller.Register(new RegisterViewModel { Email = "e@d.com" });

            // Assert
            result.AssertViewRendered().ForView(string.Empty);
            var view = result as ViewResult;
            Assert.NotNull(view);
            Assert.Equal(1, controller.ModelState.Count);
            Assert.True(controller.ModelState.ContainsKey("Email"));
        }
    }
}