using System;
using Moq;
using MvcContrib.TestHelper;
using Raven.Client;
using Raven.Client.Embedded;
using SnittListan.Controllers;
using SnittListan.Events;
using SnittListan.Models;
using SnittListan.Services;
using Xunit;
using System.Linq.Expressions;

namespace SnittListan.Test
{
	public class AccountControllerTest : IDisposable
	{
		private readonly IDocumentStore store;
		private readonly IDocumentSession sess;

		public AccountControllerTest()
		{
			store = new EmbeddableDocumentStore
			{
				RunInMemory = true
			}.Initialize();
			sess = store.OpenSession();
		}

		public void Dispose()
		{
			sess.Dispose();
		}

		[Fact]
		public void ShouldInitializeNewUser()
		{
			NewUserCreatedEvent ev = null;
			using (DomainEvent.TestWith(e => ev = (NewUserCreatedEvent)e))
			{
				var controller = new AccountController(sess, Mock.Of<IFormsAuthenticationService>());
				controller.Register(new RegisterModel
				{
					FirstName = "first name",
					LastName = "last name",
					Email = "email",
				});
			}

			Assert.NotNull(ev);
		}

		[Fact]
		public void ShouldCreateInitializedUser()
		{
			using (DomainEvent.Disable())
			{
				var controller = new AccountController(sess, Mock.Of<IFormsAuthenticationService>());
				controller.Register(new RegisterModel
				{
					FirstName = "first name",
					LastName = "last name",
					Email = "email",
				});

				var user = sess.Load<User>(1);
				Assert.NotNull(user);
				Assert.Equal("first name", user.FirstName);
				Assert.Equal("last name", user.LastName);
				Assert.Equal("email", user.Email);
				Assert.False(user.IsActive);
			}
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
				var controller = new AccountController(sess, authService);
				var result = controller.Register(new RegisterModel
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
				var controller = new AccountController(sess, Mock.Of<IFormsAuthenticationService>());
				var result = controller.Register(new RegisterModel
				{
					FirstName = "f",
					LastName = "l",
					Email = "email"
				});

				result.AssertActionRedirect().ToAction("RegisterSuccess");
			}
		}

		[Fact]
		public void ActiveUserCanLogOn()
		{
			Assert.False(true, "Not finished");
		}

		[Fact]
		public void InactiveUserCannotLogin()
		{
			var user = new User(firstName: "f", lastName: "l", email: "e@d.com", password: "pwd");
			sess.Store(user);
			sess.SaveChanges();

			var controller = new AccountController(sess, Mock.Of<IFormsAuthenticationService>());
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
				var controller = new AccountController(sess, Mock.Of<IFormsAuthenticationService>());
				controller.Register(new RegisterModel
				{
					FirstName = Guid.NewGuid().ToString(),
					LastName = Guid.NewGuid().ToString(),
					Email = "someone@microsoft.com"
				});
			}

			// make sure user is active
			sess.Load<User>(ev.User.Id).IsActive = true;
			sess.SaveChanges();

			Assert.False(true, "Not finished");
		}

		[Fact]
		public void ChangePasswordInvalidPassword()
		{
			Assert.False(true, "Not finished");
		}

		[Fact]
		public void ChangePasswordSuccess()
		{
			Assert.False(true, "Not finished");
		}
	}
}
