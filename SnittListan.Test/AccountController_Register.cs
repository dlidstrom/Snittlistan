using System;
using Moq;
using System.Linq;
using MvcContrib.TestHelper;
using SnittListan.Controllers;
using SnittListan.Events;
using SnittListan.Helpers;
using SnittListan.Models;
using SnittListan.Services;
using SnittListan.ViewModels;
using Xunit;

namespace SnittListan.Test
{
	public class AccountController_Register : DbTest
	{
		[Fact]
		public void ShouldInitializeNewUser()
		{
			NewUserCreatedEvent ev = null;
			using (DomainEvent.TestWith(e => ev = (NewUserCreatedEvent)e))
			{
				var controller = new AccountController(Session, Mock.Of<IFormsAuthenticationService>());
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
			var controller = new AccountController(Session, Mock.Of<IFormsAuthenticationService>());
			using (DomainEvent.Disable())
				controller.Register(new RegisterViewModel
				{
					FirstName = "first name",
					LastName = "last name",
					Email = "email",
				});

			var user = Session.FindUserByEmail("email").SingleOrDefault();
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
				var authService = Mock.Of<IFormsAuthenticationService>();

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
				var controller = new AccountController(Session, Mock.Of<IFormsAuthenticationService>());
				var result = controller.Register(new RegisterViewModel
				{
					FirstName = "f",
					LastName = "l",
					Email = "email"
				});

				result.AssertActionRedirect().ToAction("RegisterSuccess");
			}
		}
	}
}
