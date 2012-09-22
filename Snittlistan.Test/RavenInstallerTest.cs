namespace Snittlistan.Test
{
    using Castle.Windsor;

    using Raven.Client;

    using Snittlistan.Web.Infrastructure.Installers;

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
            Assert.NotNull(store);
            container.Release(store);
        }
    }
}
