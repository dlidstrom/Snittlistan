using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using Moq;
using NUnit.Framework;
using Raven.Client;
using Snittlistan.Web;
using Snittlistan.Web.Infrastructure.AutoMapper;
using Snittlistan.Web.Infrastructure.Indexes;
using Snittlistan.Web.Infrastructure.Installers;
using Snittlistan.Web.Models;

namespace Snittlistan.Test
{
    public abstract class DbTest
    {
        protected IDocumentStore Store;

        [SetUp]
        public void SetUp()
        {
            var container = new WindsorContainer().Install(new RavenInstaller(DocumentStoreMode.InMemory), new AutoMapperInstaller());

            // configure AutoMapper too
            AutoMapperConfiguration.Configure(container);

            Store = container.Resolve<IDocumentStore>();
            IndexCreator.CreateIndexes(Store);
            Session = Store.OpenSession();
        }

        protected IDocumentSession Session { get; private set; }

        [TearDown]
        public void TearDown()
        {
            OnTearDown();
            Session.Dispose();
            Store.Dispose();
        }

        protected static UrlHelper CreateUrlHelper()
        {
            var context = Mock.Of<HttpContextBase>();
            var routes = new RouteCollection();
            new RouteConfig(routes).Configure();
            return new UrlHelper(new RequestContext(Mock.Get(context).Object, new RouteData()), routes);
        }

        protected virtual void OnTearDown()
        {
        }

        protected User CreateActivatedUser(string firstName, string lastName, string email, string password)
        {
            var user = new User(firstName, lastName, email, password);
            user.Activate();
            Session.Store(user);
            Session.SaveChanges();
            return user;
        }
    }
}