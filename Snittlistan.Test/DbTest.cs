using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using Moq;
using Raven.Client;
using Raven.Client.Embedded;
using Snittlistan.Infrastructure;
using Snittlistan.Installers;
using Snittlistan.Models;

namespace Snittlistan.Test
{
	public abstract class DbTest : IDisposable
	{
		private readonly IDocumentStore store;

		public DbTest()
		{
			// configure AutoMapper too
			AutoMapperConfiguration
				.Configure(new WindsorContainer().Install(new AutoMapperInstaller()));
			store = new EmbeddableDocumentStore
			{
				RunInMemory = true
			}.Initialize();
			Session = store.OpenSession();
		}

		public IDocumentSession Session { get; private set; }

		public void Dispose()
		{
			Session.Dispose();
		}

		public void WaitForNonStaleResults<T>() where T : class
		{
			Session.Query<T>().Customize(x => x.WaitForNonStaleResults()).ToList();
		}

		public UrlHelper CreateUrlHelper()
		{
			var context = Mock.Of<HttpContextBase>();
			var routes = new RouteCollection();
			new RouteConfigurator(routes).Configure();
			return new UrlHelper(new RequestContext(Mock.Get(context).Object, new RouteData()), routes);
		}

		public User CreateActivatedUser(string firstName, string lastName, string email, string password)
		{
			var user = new User(
				firstName: firstName,
				lastName: lastName,
				email: email,
				password: password);
			user.Activate();
			Session.Store(user);
			Session.SaveChanges();
			WaitForNonStaleResults<User>();
			return user;
		}
	}
}
