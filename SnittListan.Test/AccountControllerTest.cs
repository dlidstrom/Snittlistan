using Moq;
using Raven.Client;
using Raven.Client.Embedded;
using SnittListan.Controllers;
using SnittListan.Events;
using SnittListan.Models;
using SnittListan.Services;
using Xunit;
using System;
using System.Web.Mvc;
using MvcContrib.TestHelper;

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

				result.AssertActionRedirect().ToAction("success");
			}
		}
	}
}
