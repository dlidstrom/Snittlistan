using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using Castle.Core;
using Castle.Core.Internal;
using Castle.Windsor;
using NUnit.Framework;
using Snittlistan.Web.Infrastructure.Installers;

namespace Snittlistan.Test
{
    [TestFixture]
    public class ControllersInstallerTest
    {
        private readonly IWindsorContainer container;

        public ControllersInstallerTest()
        {
            container = new WindsorContainer()
                .Install(new ControllerInstaller())
                .Install(new ApiControllerInstaller());
        }

        [Test]
        public void AllControllersImplementIController()
        {
            var allHandlers = InstallerTestHelper.GetAllHandlers(container);
            var controllerHandlers = InstallerTestHelper.GetAssignableHandlers(typeof(IController), container);
            var apiControllerHandlers = InstallerTestHelper.GetAssignableHandlers(typeof(IHttpController), container);
            Assert.That(allHandlers, Is.Not.Empty);
            var handlers = controllerHandlers.Concat(apiControllerHandlers)
                                             .ToArray();
            Assert.That(handlers, Is.EqualTo(allHandlers));
        }

        [Test]
        public void AllControllersAreRegistered()
        {
            // Is<TType> is a helper extension method from Windsor
            // which behaves like 'is' keyword in C# but at a Type, not instance level
            var allControllers = InstallerTestHelper.GetPublicClassesFromApplicationAssembly(c => c.Is<IController>());
            var registeredControllers = InstallerTestHelper.GetImplementationTypesFor(typeof(IController), container);
            Assert.That(registeredControllers, Is.EqualTo(allControllers));
        }

        [Test]
        public void AllAndOnlyControllersHaveControllerSuffix()
        {
            var allControllers = InstallerTestHelper.GetPublicClassesFromApplicationAssembly(c => c.Name.EndsWith("Controller"));
            var registeredControllers = InstallerTestHelper.GetImplementationTypesFor(typeof(IController), container);
            var registeredApiControllers = InstallerTestHelper.GetImplementationTypesFor(typeof(IHttpController), container);
            var actual = registeredControllers.Concat(registeredApiControllers)
                                              .OrderBy(x => x.Name)
                                              .ToArray();
            Assert.That(actual, Is.EqualTo(allControllers));
        }

        [Test]
        public void AllAndOnlyControllersLiveInControllersNamespace()
        {
            var allControllers = InstallerTestHelper.GetPublicClassesFromApplicationAssembly(c => c.Namespace.Contains("Controllers"));
            var registeredControllers = InstallerTestHelper.GetImplementationTypesFor(typeof(IController), container);
            var registeredApiControllers = InstallerTestHelper.GetImplementationTypesFor(typeof(IHttpController), container);
            var actual = registeredControllers.Concat(registeredApiControllers)
                                              .OrderBy(x => x.Name)
                                              .ToArray();
            Assert.That(actual, Is.EqualTo(allControllers));
        }

        [Test]
        public void AllControllersAreTransient()
        {
            var nonTransientControllers = InstallerTestHelper.GetAssignableHandlers(typeof(IController), container)
                .Where(c => c.ComponentModel.LifestyleType != LifestyleType.Transient)
                .ToArray();
            Assert.That(nonTransientControllers, Is.Empty);
        }

        [Test]
        public void AllControllersExposeThemselvesAsService()
        {
            var controllersWithWrongName = InstallerTestHelper.GetAssignableHandlers(typeof(IController), container)
                .Where(c => !c.ComponentModel.Services.SequenceEqual(new[] { c.ComponentModel.Implementation }))
                .ToArray();
            Assert.That(controllersWithWrongName, Is.Empty);
        }
    }
}