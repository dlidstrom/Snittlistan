using Xunit;
using SnittListan.Controllers;
using Moq;
using SnittListan.Services;
using SnittListan.Models;
using MvcContrib.TestHelper;
using System.Web.Mvc;
using SnittListan.ViewModels;

namespace SnittListan.Test
{
	public class AccountController_ChangePassword : DbTest
	{
		[Fact]
		public void InvalidUserFails()
		{
			// add a user
			var user = new User("F", "L", "e@d.com", "password");
			Session.Store(user);
			Session.SaveChanges();

			var controller = new AccountController(Session, Mock.Of<IFormsAuthenticationService>());
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
			// add a user
			var user = new User("F", "L", "e@d.com", "password");
			Session.Store(user);
			Session.SaveChanges();

			var controller = new AccountController(Session, Mock.Of<IFormsAuthenticationService>());
			var result = controller.ChangePassword(new ChangePasswordViewModel
			{
				Email = "e@d.com",
				NewPassword = "newpass",
				ConfirmPassword = "newpass"
			});

			result.AssertActionRedirect().ToAction("ChangePasswordSuccess");

			// also make sure password actually changed
			Assert.True(Session.Load<User>(1).ValidatePassword("newpass"), "Password was not changed");
		}

		[Fact]
		public void ChangePasswordSuccessReturnsView()
		{
			var controller = new AccountController(Session, Mock.Of<IFormsAuthenticationService>());
			var result = controller.ChangePasswordSuccess();
			result.AssertViewRendered().ForView(string.Empty);
		}
	}
}
