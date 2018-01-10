using Castle.Windsor;
using NUnit.Framework;
using Raven.Client;
using Snittlistan.Web.Infrastructure.Installers;

namespace Snittlistan.Test
{
    [TestFixture]
    public class RavenInstallerTest
    {
        private readonly IWindsorContainer container;

        public RavenInstallerTest()
        {
            container = new WindsorContainer().Install(new RavenInstaller(DocumentStoreMode.InMemory));
        }

        [Test]
        public void InstallsDocumentStore()
        {
            var store = container.Resolve<IDocumentStore>();
            Assert.NotNull(store);
            container.Release(store);
        }
    }
}