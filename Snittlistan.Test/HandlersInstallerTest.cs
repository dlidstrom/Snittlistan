namespace Snittlistan.Test
{
    using System.Linq;
    using Castle.Core;
    using Castle.Windsor;
    using Events;
    using Handlers;
    using Infrastructure.Installers;
    using MvcContrib.TestHelper;
    using Xunit;

    public class HandlersInstallerTest
    {
        private readonly IWindsorContainer container;

        public HandlersInstallerTest()
        {
            container = new WindsorContainer().Install(new HandlersInstaller());
        }

        [Fact]
        public void InstallsHandlerForNewUserCreatedEvent()
        {
            var handler = InstallerTestHelper.GetHandlersFor(typeof(IHandle<NewUserCreatedEvent>), container);
            handler.Length.ShouldBe(1);
        }

        [Fact]
        public void HandlersAreTransient()
        {
            var nonTransientControllers = InstallerTestHelper.GetHandlersFor(typeof(IHandle<>), container)
                .Where(c => c.ComponentModel.LifestyleType != LifestyleType.Transient)
                .ToArray();
            Assert.Empty(nonTransientControllers);
        }
    }
}