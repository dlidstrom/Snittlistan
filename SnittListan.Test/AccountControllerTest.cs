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
		[Fact]
		public void ShouldInitializeNewUser()
		{
			//var session = Mock.Of<IDocumentSession>();
			//var store = Mock.Of<EmbeddableDocumentStore>(s => s.OpenSession() == session);
			var store = new EmbeddableDocumentStore
			{
			    RunInMemory = true
			}.Initialize();

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
	}
}
