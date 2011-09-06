using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using MvcContrib.TestHelper;
using Snittlistan.Controllers;
using Snittlistan.Models;
using Snittlistan.Services;
using Snittlistan.ViewModels;
using Xunit;

namespace Snittlistan.Test
{
	public class AccountController_Logon : DbTest
	{
		[Fact]
		public void UnknownUserCannotLogon()
		{
			var service = Mock.Of<IAuthenticationService>();
			bool cookieSet = false;
			Mock.Get(service)
				.Setup(x => x.SetAuthCookie(It.IsAny<string>(), It.IsAny<bool>()))
				.Callback(() => cookieSet = true);

			var controller = new AccountController(Session, service);
			var result = controller.LogOn(new LogOnViewModel { Email = "unknown@d.com", Password = "some pwd" }, string.Empty);

			result.AssertViewRendered().ForView(string.Empty);
			cookieSet.ShouldBe(false);
		}

		[Fact]
		public void ActiveUserCanLogon()
		{
			bool cookieSet = false;
			Action cookieSetAction = () => cookieSet = true;
			var controller = SetupPasswordTest(cookieSetAction);

			var result = controller.LogOn(new LogOnViewModel { Email = "e@d.com", Password = "some pwd" }, string.Empty);
			controller.ModelState.ContainsKey("Email").ShouldBe(false);
			controller.ModelState.ContainsKey("Password").ShouldBe(false);
			result.AssertActionRedirect().ToController("Home").ToAction("Index");

			cookieSet.ShouldBe(true);
		}

		[Fact]
		public void InactiveUserCannotLogon()
		{
			Session.Store(new User(firstName: "f", lastName: "l", email: "e@d.com", password: "pwd"));
			Session.SaveChanges();
			WaitForNonStaleResults<User>();

			bool loggedOn = false;
			var service = Mock.Of<IAuthenticationService>();
			Mock.Get(service)
				.Setup(s => s.SetAuthCookie(It.IsAny<string>(), It.IsAny<bool>()))
				.Callback(() => loggedOn = true);
			var controller = new AccountController(Session, service);
			controller.Url = CreateUrlHelper();
			var result = controller.LogOn(new LogOnViewModel { Email = "e@d.com", Password = "pwd" }, string.Empty);
			controller.ModelState.Keys.Contains("Inactive").ShouldBe(true);
			result.AssertViewRendered().ForView(string.Empty);
			loggedOn.ShouldBe(false);
		}

		[Fact]
		public void WrongPasswordRedisplaysForm()
		{
			bool cookieSet = false;
			Action cookieSetAction = () => cookieSet = true;
			var controller = SetupPasswordTest(cookieSetAction);

			var result = controller.LogOn(new LogOnViewModel { Email = "e@d.com", Password = "some other pwd" }, string.Empty);

			controller.ModelState.ContainsKey("Email").ShouldBe(false);
			controller.ModelState.ContainsKey("Password").ShouldBe(true);
			cookieSet.ShouldBe(false);
			result.AssertViewRendered().ForView(string.Empty);
		}

		private AccountController SetupPasswordTest(Action cookieSetAction)
		{
			// create an active user
			var user = CreateActivatedUser("F", "L", "e@d.com", "some pwd");

			var service = Mock.Of<IAuthenticationService>();
			Mock.Get(service)
				.Setup(x => x.SetAuthCookie(It.Is<string>(s => s == "e@d.com"), It.Is<bool>(b => b == false)))
				.Callback(cookieSetAction);

			var controller = new AccountController(Session, service);
			controller.Url = CreateUrlHelper();
			return controller;
		}
	}
}
