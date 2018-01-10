using System.Linq;
using Castle.Core;
using Castle.Windsor;
using NUnit.Framework;
using Snittlistan.Web.DomainEvents;
using Snittlistan.Web.Handlers;
using Snittlistan.Web.Infrastructure.Installers;

namespace Snittlistan.Test
{
    [TestFixture]
    public class HandlersInstallerTest
    {
        private readonly IWindsorContainer container;

        public HandlersInstallerTest()
        {
            container = new WindsorContainer().Install(new HandlersInstaller());
        }

        [Test]
        public void InstallsHandlerForNewUserCreatedEvent()
        {
            var handler = InstallerTestHelper.GetHandlersFor(typeof(IHandle<NewUserCreatedEvent>), container);
            Assert.That(handler.Length, Is.EqualTo(1));
        }

        [Test]
        public void HandlersAreTransient()
        {
            var nonTransientControllers = InstallerTestHelper.GetHandlersFor(typeof(IHandle<>), container)
                .Where(c => c.ComponentModel.LifestyleType != LifestyleType.Transient)
                .ToArray();
            Assert.That(nonTransientControllers, Is.Empty);
        }
    }
}