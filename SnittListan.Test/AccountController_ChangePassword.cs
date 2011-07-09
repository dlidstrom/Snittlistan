using System;
using System.Linq;
using System.Web.Mvc;
using Moq;
using SnittListan.Helpers;
using MvcContrib.TestHelper;
using SnittListan.Controllers;
using SnittListan.Models;
using SnittListan.Services;
using SnittListan.ViewModels;
using Xunit;

namespace SnittListan.Test
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
			}).AssertResultIs<HttpNotFoundResult>();
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
			var controller = new AccountController(Session, Mock.Of<IFormsAuthenticationService>());
			var result = controller.ChangePasswordSuccess();
			result.AssertViewRendered().ForView(string.Empty);
		}

		/// <summary>
		/// Creates a user with random password.
		/// </summary>
		/// <returns></returns>
		private AccountController CreateUserAndController(string email)
		{
			// add a user
			var user = new User("F", "L", email, password: Guid.NewGuid().ToString());
			Session.Store(user);
			Session.SaveChanges();

			return new AccountController(Session, Mock.Of<IFormsAuthenticationService>());
		}
	}
}
