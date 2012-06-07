namespace Snittlistan.Test
{
    using Castle.Windsor;
    using Installers;
    using MvcContrib.TestHelper;
    using Raven.Client;
    using Xunit;

    public class RavenInstallerTest
    {
        private readonly IWindsorContainer container;

        public RavenInstallerTest()
        {
            container = new WindsorContainer().Install(new RavenInstaller(DocumentStoreMode.InMemory));
        }

        [Fact]
        public void InstallsDocumentStore()
        {
            var store = container.Resolve<IDocumentStore>();
            store.ShouldNotBeNull("Expected IDocumentStore in container");
            container.Release(store);
        }
    }
}
