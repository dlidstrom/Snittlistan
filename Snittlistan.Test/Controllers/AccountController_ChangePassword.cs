using System;
using Moq;
using Snittlistan.Web.Areas.V1.Controllers;
using Snittlistan.Web.Areas.V1.ViewModels.Account;
using Snittlistan.Web.Models;
using Snittlistan.Web.Services;
using Xunit;

namespace Snittlistan.Test.Controllers
{
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