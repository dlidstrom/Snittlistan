using System;
using System.Web.Mvc;
using Moq;
using MvcContrib.TestHelper;
using SnittListan.Controllers;
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
			var controller = new AccountController(Session, Mock.Of<IFormsAuthenticationService>());
			var result = controller.Verify(Guid.NewGuid());
			result.AssertActionRedirect().ToAction("Register");
		}

		[Fact]
		public void KnownUserIsActivatedAndShownSuccess()
		{
			var user = new User("F", "L", "e@d.com", "some pwd");
			Session.Store(user);
			Session.SaveChanges();

			VerifyActivateForUser(user)
				.AssertActionRedirect()
				.ToAction("VerifySuccess");
		}

		[Fact]
		public void ActivatedUserRedirectsToLogOn()
		{
			var user = new User("F", "L", "e@d.com", "some pwd");
			user.Activate();
			Session.Store(user);
			Session.SaveChanges();

			VerifyActivateForUser(user)
				.AssertActionRedirect()
				.ToAction("LogOn");
		}

		private ActionResult VerifyActivateForUser(User user)
		{
			bool loggedSomebodyOn = false;
			var service = Mock.Of<IFormsAuthenticationService>();
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
