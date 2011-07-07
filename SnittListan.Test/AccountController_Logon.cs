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
			// create an active user
			var user = new User("F", "L", "e@d.com", "some pwd");
			user.Activate();
			Session.Store(user);
			Session.SaveChanges();

			// make sure SetAuthCookie is set
			bool cookieSet = false;
			var service = Mock.Of<IFormsAuthenticationService>();
			Mock.Get(service)
				.Setup(x => x.SetAuthCookie(It.Is<string>(s => s == "e@d.com"), It.Is<bool>(b => b == false)))
				.Callback(() => cookieSet = true);

			var controller = new AccountController(Session, service);
			var result = controller.LogOn(new LogOnModel { Email = "e@d.com", Password = "some pwd" }, string.Empty);
			controller.ModelState.ContainsKey("Email").ShouldBe(false);
			controller.ModelState.ContainsKey("Password").ShouldBe(false);
			result.AssertActionRedirect().ToController("Home").ToAction("Index");

			cookieSet.ShouldBe(true);
		}

		[Fact]
		public void InactiveUserCannotLogon()
		{
			var user = new User(firstName: "f", lastName: "l", email: "e@d.com", password: "pwd");
			Session.Store(user);
			Session.SaveChanges();

			var controller = new AccountController(Session, Mock.Of<IFormsAuthenticationService>());
			controller.LogOn(new LogOnModel { Email = "e@d.com", Password = "pwd" }, null);

			Assert.False(true, "Not finished");
		}

		[Fact]
		public void WrongPasswordRedisplaysForm()
		{
			Assert.False(true, "Not finished");
		}

		[Fact]
		public void CanLoginVerifiedUserWithEmailAddress()
		{
			NewUserCreatedEvent ev = null;
			using (DomainEvent.TestWith(e => ev = (NewUserCreatedEvent)e))
			{
				var controller = new AccountController(Session, Mock.Of<IFormsAuthenticationService>());
				controller.Register(new RegisterModel
				{
					FirstName = Guid.NewGuid().ToString(),
					LastName = Guid.NewGuid().ToString(),
					Email = "someone@microsoft.com"
				});
			}

			// make sure user is active
			Session.Load<User>(ev.User.Id).Activate();
			Session.SaveChanges();

			Assert.False(true, "Not finished");
		}
	}
}
