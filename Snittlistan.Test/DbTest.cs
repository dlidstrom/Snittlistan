using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using Moq;
using Raven.Client;
using Raven.Client.Embedded;
using Snittlistan.Web.App_Start;
using Snittlistan.Web.Infrastructure.AutoMapper;
using Snittlistan.Web.Infrastructure.Installers;
using Snittlistan.Web.Models;

namespace Snittlistan.Test
{
    public abstract class DbTest : IDisposable
    {
        protected readonly IDocumentStore Store;

        protected DbTest()
        {
            // configure AutoMapper too
            AutoMapperConfiguration
                .Configure(new WindsorContainer().Install(new AutoMapperInstaller()));

            Store = new EmbeddableDocumentStore { RunInMemory = true };

            // initialize
            RavenInstaller.InitializeStore(Store, DocumentStoreMode.InMemory);
            Session = Store.OpenSession();
        }

        protected IDocumentSession Session { get; private set; }

        protected virtual void OnDispose()
        {
        }

        public void Dispose()
        {
            this.OnDispose();
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

        protected User CreateActivatedUser(string firstName, string lastName, string email, string password)
        {
            var user = new User(
                firstName: firstName,
                lastName: lastName,
                email: email,
                password: password);
            user.Activate(false);
            Session.Store(user);
            Session.SaveChanges();
            return user;
        }
    }
}