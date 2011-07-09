using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using Raven.Client;
using Raven.Client.Embedded;
using SnittListan.Models;

namespace SnittListan.Test
{
	public abstract class DbTest : IDisposable
	{
		private readonly IDocumentStore store;

		public DbTest()
		{
			store = new EmbeddableDocumentStore
			{
				RunInMemory = true
			}.Initialize();
			Session = store.OpenSession();

			// this is a workaround
			// to be able to query, at least one document must be stored
			Session.Store(new User("Daniel", "Lidström", "someone@somedomain.com", "some password"));
			Session.SaveChanges();
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
	}
}
