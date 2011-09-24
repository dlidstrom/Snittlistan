using System;
using System.Linq;
using Moq;
using MvcContrib.TestHelper;
using Snittlistan.Controllers;
using Snittlistan.Helpers;
using Snittlistan.Models;
using Snittlistan.Services;
using Snittlistan.ViewModels;
using Xunit;

namespace Snittlistan.Test
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
		public void ChangePasswordSuccess()
		{
			var controller = CreateUserAndController("e@d.com");
			var result = controller.ChangePassword(new ChangePasswordViewModel
			{
				Email = "e@d.com",
				NewPassword = "newpass",
				ConfirmPassword = "newpass"
			});

			result.AssertActionRedirect().ToAction("ChangePasswordSuccess");

			// also make sure password actually changed
			var user = Session.FindUserByEmail("e@d.com").SingleOrDefault();
			user.ShouldNotBeNull("Did not find user");
			user.ValidatePassword("newpass").ShouldBe(true);
		}

		[Fact]
		public void ChangePasswordSuccessReturnsView()
		{
			var controller = new AccountController(Session, Mock.Of<IAuthenticationService>());
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
			Session.Store(new User("F", "L", email, password: Guid.NewGuid().ToString()));
			Session.SaveChanges();
			WaitForNonStaleResults<User>();

			return new AccountController(Session, Mock.Of<IAuthenticationService>());
		}
	}
}
