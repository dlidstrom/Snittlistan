using System;
using Moq;
using MvcContrib.TestHelper;
using SnittListan.Controllers;
using SnittListan.Services;
using Xunit;
using SnittListan.Models;

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
		public void KnownUserIsActivatedAndCanLogOn()
		{
			var user = new User("F", "L", "e@d.com", "some pwd");
			Session.Store(user);
			Session.SaveChanges();

			VerifyActivateForUser(user);
		}

		[Fact]
		public void ActivatedUserRedirectsToLogOn()
		{
			var user = new User("F", "L", "e@d.com", "some pwd");
			user.Activate();
			Session.Store(user);
			Session.SaveChanges();

			VerifyActivateForUser(user);
		}

		private void VerifyActivateForUser(User user)
		{
			bool loggedSomebodyOn = false;
			var service = Mock.Of<IFormsAuthenticationService>();
			Mock.Get(service)
				.Setup(s => s.SetAuthCookie(It.IsAny<string>(), It.IsAny<bool>()))
				.Callback(() => loggedSomebodyOn = true);
			var controller = new AccountController(Session, service);
			var result = controller.Verify(Guid.Parse(user.ActivationKey));
			result.AssertActionRedirect().ToAction("VerifySuccess");
			loggedSomebodyOn.ShouldBe(false);
		}
	}
}
