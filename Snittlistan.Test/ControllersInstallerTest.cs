using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using Castle.Core;
using Castle.Core.Internal;
using Castle.Windsor;
using Snittlistan.Web.Infrastructure.Installers;
using Xunit;

namespace Snittlistan.Test
{
    public class ControllersInstallerTest
    {
        private readonly IWindsorContainer container;

        public ControllersInstallerTest()
        {
            container = new WindsorContainer()
                .Install(new ControllerInstaller())
                .Install(new ApiControllerInstaller());
        }

        [Fact]
        public void AllControllersImplementIController()
        {
            var allHandlers = InstallerTestHelper.GetAllHandlers(container);
            var controllerHandlers = InstallerTestHelper.GetAssignableHandlers(typeof(IController), container);
            var apiControllerHandlers = InstallerTestHelper.GetAssignableHandlers(typeof(IHttpController), container);
            Assert.NotEmpty(allHandlers);
            var handlers = controllerHandlers.Concat(apiControllerHandlers)
                                             .ToArray();
            Assert.Equal(allHandlers, handlers);
        }

        [Fact]
        public void AllControllersAreRegistered()
        {
            // Is<TType> is a helper extension method from Windsor
            // which behaves like 'is' keyword in C# but at a Type, not instance level
            var allControllers = InstallerTestHelper.GetPublicClassesFromApplicationAssembly(c => c.Is<IController>());
            var registeredControllers = InstallerTestHelper.GetImplementationTypesFor(typeof(IController), container);
            Assert.Equal(allControllers, registeredControllers);
        }

        [Fact]
        public void AllAndOnlyControllersHaveControllerSuffix()
        {
            var allControllers = InstallerTestHelper.GetPublicClassesFromApplicationAssembly(c => c.Name.EndsWith("Controller"));
            var registeredControllers = InstallerTestHelper.GetImplementationTypesFor(typeof(IController), container);
            var registeredApiControllers = InstallerTestHelper.GetImplementationTypesFor(typeof(IHttpController), container);
            var actual = registeredControllers.Concat(registeredApiControllers)
                                              .OrderBy(x => x.Name)
                                              .ToArray();
            Assert.Equal(allControllers, actual);
        }

        [Fact]
        public void AllAndOnlyControllersLiveInControllersNamespace()
        {
            var allControllers = InstallerTestHelper.GetPublicClassesFromApplicationAssembly(c => c.Namespace.Contains("Controllers"));
            var registeredControllers = InstallerTestHelper.GetImplementationTypesFor(typeof(IController), container);
            var registeredApiControllers = InstallerTestHelper.GetImplementationTypesFor(typeof(IHttpController), container);
            var actual = registeredControllers.Concat(registeredApiControllers)
                                              .OrderBy(x => x.Name)
                                              .ToArray();
            Assert.Equal(allControllers, actual);
        }

        [Fact]
        public void AllControllersAreTransient()
        {
            var nonTransientControllers = InstallerTestHelper.GetAssignableHandlers(typeof(IController), container)
                .Where(c => c.ComponentModel.LifestyleType != LifestyleType.Transient)
                .ToArray();
            Assert.Empty(nonTransientControllers);
        }

        [Fact]
        public void AllControllersExposeThemselvesAsService()
        {
            var controllersWithWrongName = InstallerTestHelper.GetAssignableHandlers(typeof(IController), container)
                .Where(c => !c.ComponentModel.Services.SequenceEqual(new[] { c.ComponentModel.Implementation }))
                .ToArray();
            Assert.Empty(controllersWithWrongName);
        }
    }
}