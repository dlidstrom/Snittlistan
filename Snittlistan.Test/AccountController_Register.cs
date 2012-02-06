namespace Snittlistan.Test
{
    using System;
    using System.Web.Mvc;
    using Moq;
    using MvcContrib.TestHelper;
    using Snittlistan.Controllers;
    using Snittlistan.Events;
    using Snittlistan.Helpers;
    using Snittlistan.Models;
    using Snittlistan.Services;
    using Snittlistan.ViewModels.Account;
    using Xunit;

    public class AccountController_Register : DbTest
    {
        [Fact]
        public void ShouldInitializeNewUser()
        {
            NewUserCreatedEvent ev = null;
            using (DomainEvent.TestWith(e => ev = (NewUserCreatedEvent)e))
            {
                var controller = new AccountController(Session, Mock.Of<IAuthenticationService>());
                controller.Register(new RegisterViewModel
                {
                    FirstName = "first name",
                    LastName = "last name",
                    Email = "email",
                });
            }

            ev.ShouldNotBeNull("No event raised");
        }

        [Fact]
        public void ShouldCreateInitializedUser()
        {
            var controller = new AccountController(Session, Mock.Of<IAuthenticationService>());
            using (DomainEvent.Disable())
                controller.Register(new RegisterViewModel
                {
                    FirstName = "first name",
                    LastName = "last name",
                    Email = "email",
                });

            // normally done by infrastructure (special action filter)
            Session.SaveChanges();

            // let indexing do its job
            WaitForNonStaleResults<User>();

            var user = Session.FindUserByEmail("email");
            user.ShouldNotBeNull("Should find it");
            user.FirstName.ShouldBe("first name");
            user.LastName.ShouldBe("last name");
            user.Email.ShouldBe("email");
            user.IsActive.ShouldBe(false);
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
                var controller = new AccountController(Session, authService);
                var result = controller.Register(new RegisterViewModel
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
                var controller = new AccountController(Session, Mock.Of<IAuthenticationService>());
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
            var controller = new AccountController(Session, Mock.Of<IAuthenticationService>());

            // Act
            var result = controller.Register(new RegisterViewModel { Email = "e@d.com" });

            // Assert
            result.AssertViewRendered().ForView(string.Empty);
            var view = result as ViewResult;
            view.ShouldNotBeNull("Expected ViewResult");
            controller.ModelState.Count.ShouldBe(1);
            controller.ModelState.ContainsKey("Email").ShouldBe(true);
        }
    }
}