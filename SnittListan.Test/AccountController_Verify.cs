using System;
using System.Linq;
using System.Web.Mvc;
using Moq;
using MvcContrib.TestHelper;
using SnittListan.Controllers;
using SnittListan.Helpers;
using SnittListan.Models;
using SnittListan.Services;
using Xunit;

namespace SnittListan.Test
{
	public class AccountController_Verify : DbTest
	{
		[Fact]
		public void UnknownIdFails()
		{
			var controller = new AccountController(Session, Mock.Of<IAuthenticationService>());
			var result = controller.Verify(Guid.NewGuid());
			result.AssertActionRedirect().ToAction("Register");
		}

		[Fact]
		public void KnownUserIsActivatedAndShownSuccess()
		{
			var user = new User("F", "L", "e@d.com", "some pwd");
			Session.Store(user);
			Session.SaveChanges();

			VerifyActivationKeyForUser(user)
				.AssertActionRedirect()
				.ToAction("VerifySuccess");
			var storedUser = Session.FindUserByEmail("e@d.com").SingleOrDefault();
			storedUser.ShouldNotBeNull("Failed to find user");
			storedUser.IsActive.ShouldBe(true);
		}

		[Fact]
		public void ActivatedUserRedirectsToLogOn()
		{
			var user = new User("F", "L", "e@d.com", "some pwd");
			user.Activate();
			Session.Store(user);
			Session.SaveChanges();

			VerifyActivationKeyForUser(user)
				.AssertActionRedirect()
				.ToAction("LogOn");
		}

		private ActionResult VerifyActivationKeyForUser(User user)
		{
			bool loggedSomebodyOn = false;
			var service = Mock.Of<IAuthenticationService>();
			Mock.Get(service)
				.Setup(s => s.SetAuthCookie(It.IsAny<string>(), It.IsAny<bool>()))
				.Callback(() => loggedSomebodyOn = true);
			var controller = new AccountController(Session, service);
			var result = controller.Verify(Guid.Parse(user.ActivationKey));
			loggedSomebodyOn.ShouldBe(false);
			return result;
		}
	}
}
