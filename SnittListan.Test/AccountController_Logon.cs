using System;
using Moq;
using MvcContrib.TestHelper;
using SnittListan.Controllers;
using SnittListan.Events;
using SnittListan.Models;
using SnittListan.Services;
using Xunit;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web;

namespace SnittListan.Test
{
	public class AccountController_Logon : DbTest
	{
		private UrlHelper CreateUrlHelper()
		{
			var context = Mock.Of<HttpContextBase>();
			var routes = new RouteCollection();
			new RouteConfigurator(routes).Configure();
			return new UrlHelper(new RequestContext(Mock.Get(context).Object, new RouteData()), routes);
		}

		[Fact]
		public void UnknownUserCannotLogon()
		{
			// Arrange


			// Act

			// Assert
			Assert.False(false, "Not finished yet");
		}

		[Fact]
		public void ActiveUserCanLogon()
		{
			bool cookieSet = false;
			Action cookieSetAction = () => cookieSet = true;
			AccountController controller = SetupPasswordTest(cookieSetAction);

			var result = controller.LogOn(new LogOnModel { Email = "e@d.com", Password = "some pwd" }, string.Empty);
			controller.ModelState.ContainsKey("Email").ShouldBe(false);
			controller.ModelState.ContainsKey("Password").ShouldBe(false);
			result.AssertActionRedirect().ToController("Home").ToAction("Index");

			cookieSet.ShouldBe(true);
		}

		private AccountController SetupPasswordTest(Action cookieSetAction)
		{
			// create an active user
			var user = new User("F", "L", "e@d.com", "some pwd");
			user.Activate();
			Session.Store(user);
			Session.SaveChanges();

			var service = Mock.Of<IFormsAuthenticationService>();
			Mock.Get(service)
				.Setup(x => x.SetAuthCookie(It.Is<string>(s => s == "e@d.com"), It.Is<bool>(b => b == false)))
				.Callback(cookieSetAction);

			AccountController controller = new AccountController(Session, service);
			controller.Url = CreateUrlHelper();
			return controller;
		}

		[Fact]
		public void InactiveUserCannotLogon()
		{
			var user = new User(firstName: "f", lastName: "l", email: "e@d.com", password: "pwd");
			Session.Store(user);
			Session.SaveChanges();

			bool loggedOn = false;
			var service = Mock.Of<IFormsAuthenticationService>();
			Mock.Get(service)
				.Setup(s => s.SetAuthCookie(It.IsAny<string>(), It.IsAny<bool>()))
				.Callback(() => loggedOn = true);
			var controller = new AccountController(Session, service);
			controller.Url = CreateUrlHelper();
			var result = controller.LogOn(new LogOnModel { Email = "e@d.com", Password = "pwd" }, string.Empty);
			controller.ModelState.Keys.Contains("Inactive").ShouldBe(true);
			result.AssertViewRendered().ForView(string.Empty);
			loggedOn.ShouldBe(false);
		}

		[Fact]
		public void WrongPasswordRedisplaysForm()
		{
			bool cookieSet = false;
			Action cookieSetAction = () => cookieSet = true;
			AccountController controller = SetupPasswordTest(cookieSetAction);

			var result = controller.LogOn(new LogOnModel { Email = "e@d.com", Password = "some other pwd" }, string.Empty);

			controller.ModelState.ContainsKey("Email").ShouldBe(false);
			controller.ModelState.ContainsKey("Password").ShouldBe(true);
			cookieSet.ShouldBe(false);
			result.AssertViewRendered().ForView(string.Empty);
		}
	}
}
