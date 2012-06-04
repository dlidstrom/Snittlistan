namespace Snittlistan.Test
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Castle.Windsor;
    using Infrastructure.AutoMapper;
    using Installers;
    using Models;
    using Moq;
    using Raven.Client;
    using Raven.Client.Embedded;

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
            RavenInstaller.InitializeStore(Store);
            Session = Store.OpenSession();
        }

        protected IDocumentSession Session { get; private set; }

        public void Dispose()
        {
            Session.Dispose();
            Store.Dispose();
        }

        protected static UrlHelper CreateUrlHelper()
        {
            var context = Mock.Of<HttpContextBase>();
            var routes = new RouteCollection();
            new RouteConfigurator(routes).Configure();
            return new UrlHelper(new RequestContext(Mock.Get(context).Object, new RouteData()), routes);
        }

        protected User CreateActivatedUser(string firstName, string lastName, string email, string password)
        {
            var user = new User(
                firstName: firstName,
                lastName: lastName,
                email: email,
                password: password);
            user.Activate();
            Session.Store(user);
            Session.SaveChanges();
            return user;
        }
    }
}