namespace Snittlistan.Test
{
    using Castle.Windsor;
    using NUnit.Framework;
    using Raven.Client;
    using Snittlistan.Web.Infrastructure.Installers;

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
            IDocumentStore store = container.Resolve<IDocumentStore>();
            Assert.NotNull(store);
            container.Release(store);
        }
    }
}