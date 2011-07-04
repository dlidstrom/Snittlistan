using System;
using Moq;
using SnittListan.Controllers;
using SnittListan.Events;
using SnittListan.Models;
using SnittListan.Services;
using Xunit;

namespace SnittListan.Test
{
	public class AccountController_Logon : DbTest
	{
		[Fact]
		public void ActiveUserCanLogon()
		{
			Assert.False(true, "Not finished");
		}

		[Fact]
		public void InactiveUserCannotLogon()
		{
			var user = new User(firstName: "f", lastName: "l", email: "e@d.com", password: "pwd");
			Session.Store(user);
			Session.SaveChanges();

			var controller = new AccountController(Session, Mock.Of<IFormsAuthenticationService>());
			controller.LogOn(new LogOnModel { UserName = "e@d.com", Password = "pwd" }, null);

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
			Session.Load<User>(ev.User.Id).IsActive = true;
			Session.SaveChanges();

			Assert.False(true, "Not finished");
		}
	}
}
