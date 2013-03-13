namespace Snittlistan.Test
{
    using System;

    using Moq;

    using Snittlistan.Web.Areas.V1.Controllers;
    using Snittlistan.Web.Areas.V1.ViewModels.Account;
    using Snittlistan.Web.Controllers;
    using Snittlistan.Web.Helpers;
    using Snittlistan.Web.Models;
    using Snittlistan.Web.Services;

    using Xunit;

    public class AccountController_ChangePassword : DbTest
    {
        [Fact]
        public void InvalidUserFails()
        {
            var controller = CreateUserAndController("e@d.com");
            controller.ChangePassword(new ChangePasswordViewModel
            {
                Email = "f@d.com",
                NewPassword = "somepasswd",
                ConfirmPassword = "somepasswd"
            }).AssertViewRendered().ForView(string.Empty);
        }

        [Fact(Skip = "Password change not completed")]
        public void ChangePasswordSuccess()
        {
            var controller = CreateUserAndController("e@d.com");
            var result = controller.ChangePassword(
                new ChangePasswordViewModel
                    {
                        Email = "e@d.com",
                        NewPassword = "newpass",
                        ConfirmPassword = "newpass"
                    });

            result.AssertActionRedirect().ToAction("ChangePasswordSuccess");

            // also make sure password actually changed
            var user = Session.FindUserByEmail("e@d.com");
            Assert.NotNull(user);
            Assert.True(user.ValidatePassword("newpass"));
        }

        [Fact]
        public void ChangePasswordSuccessReturnsView()
        {
            var controller = new AccountController(Mock.Of<IAuthenticationService>()) { DocumentSession = Session };
            var result = controller.ChangePasswordSuccess();
            result.AssertViewRendered().ForView(string.Empty);
        }

        /// <summary>
        /// Creates a user with random password.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private AccountController CreateUserAndController(string email)
        {
            // add a user
            Session.Store(new User("F", "L", email, Guid.NewGuid().ToString()));
            Session.SaveChanges();

            return new AccountController(Mock.Of<IAuthenticationService>()) { DocumentSession = Session };
        }
    }
}