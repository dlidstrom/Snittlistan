#nullable enable

using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using Moq;
using NUnit.Framework;
using Raven.Client;
using Snittlistan.Web;
using Snittlistan.Web.Infrastructure.Database;
using Snittlistan.Web.Infrastructure.Indexes;
using Snittlistan.Web.Infrastructure.Installers;
using Snittlistan.Web.Models;

namespace Snittlistan.Test;

public abstract class DbTest
{
    protected IDocumentStore Store = null!;

    [SetUp]
    public void SetUp()
    {
        IWindsorContainer container = new WindsorContainer().Install(
            new RavenInstaller(Array.Empty<Tenant>(), DocumentStoreMode.InMemory));

        Store = container.Resolve<IDocumentStore>();
        IndexCreator.CreateIndexes(Store);
        Session = Store.OpenSession();
        OnSetUp();
    }

    protected virtual void OnSetUp()
    {
    }

    protected IDocumentSession Session { get; private set; } = null!;

    [TearDown]
    public void TearDown()
    {
        OnTearDown();
        Session.Dispose();
        Store.Dispose();
    }

    protected static UrlHelper CreateUrlHelper()
    {
        HttpContextBase context = Mock.Of<HttpContextBase>();
        RouteCollection routes = new();
        new RouteConfig(routes).Configure();
        return new UrlHelper(new RequestContext(Mock.Get(context).Object, new RouteData()), routes);
    }

    protected virtual void OnTearDown()
    {
    }

    protected User CreateActivatedUser(string firstName, string lastName, string email, string password)
    {
        User user = new(firstName, lastName, email, password);
        user.Activate();
        Session.Store(user);
        Session.SaveChanges();
        return user;
    }
}
