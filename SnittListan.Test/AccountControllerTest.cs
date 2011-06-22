using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using SnittListan.Controllers;
using SnittListan.Models;
using Raven.Client.Embedded;
using SnittListan.Events;
using Moq;
using Raven.Client;
using SnittListan.Services;

namespace SnittListan.Test
{
	public class AccountControllerTest
	{
		private readonly IDocumentStore store;

		public AccountControllerTest()
		{
			store = new EmbeddableDocumentStore
			{
				RunInMemory = true
			}.Initialize();
		}

		[Fact]
		public void ShouldInitializeNewUser()
		{
			NewUserCreatedEvent ev = null;
			using (DomainEvent.TestWith(e => ev = (NewUserCreatedEvent)e))
			using (var sess = store.OpenSession())
			{
				var controller = new AccountController(sess, Mock.Of<IFormsAuthenticationService>());
				controller.Register(new RegisterModel
				{
					FirstName = "first name",
					LastName = "last name",
					Email = "email",
					UserName = "username"
				});
			}

			Assert.NotNull(ev);
		}

		[Fact]
		public void ShouldCreateInitializedUser()
		{
			using (var sess = store.OpenSession())
			{
				var controller = new AccountController(sess, Mock.Of<IFormsAuthenticationService>());
				controller.Register(new RegisterModel
				{
					FirstName = "first name",
					LastName = "last name",
					Email = "email",
					UserName = "username"
				});

				sess.SaveChanges();

				var user = sess.Load<User>(1);
				Assert.NotNull(user);
				Assert.Equal("first name", user.FirstName);
				Assert.Equal("last name", user.LastName);
				Assert.Equal("email", user.Email);
				Assert.Equal("username", user.UserName);
				Assert.False(user.IsActive);
			}
		}
	}
}
