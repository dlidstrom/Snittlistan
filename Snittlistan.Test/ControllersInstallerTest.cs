namespace Snittlistan.Test
{
    using System.Linq;
    using System.Web.Http.Controllers;
    using System.Web.Mvc;
    using Castle.Core;
    using Castle.Core.Internal;
    using Castle.Windsor;
    using NUnit.Framework;
    using Snittlistan.Web.Infrastructure.Installers;

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
            Castle.MicroKernel.IHandler[] allHandlers = InstallerTestHelper.GetAllHandlers(container);
            Castle.MicroKernel.IHandler[] controllerHandlers = InstallerTestHelper.GetAssignableHandlers(typeof(IController), container);
            Castle.MicroKernel.IHandler[] apiControllerHandlers = InstallerTestHelper.GetAssignableHandlers(typeof(IHttpController), container);
            Assert.That(allHandlers, Is.Not.Empty);
            Castle.MicroKernel.IHandler[] handlers = controllerHandlers.Concat(apiControllerHandlers)
                                             .ToArray();
            Assert.That(handlers, Is.EqualTo(allHandlers));
        }

        [Test]
        public void AllControllersAreRegistered()
        {
            // Is<TType> is a helper extension method from Windsor
            // which behaves like 'is' keyword in C# but at a Type, not instance level
            System.Type[] allControllers = InstallerTestHelper.GetPublicClassesFromApplicationAssembly(c => c.Is<IController>());
            System.Type[] registeredControllers = InstallerTestHelper.GetImplementationTypesFor(typeof(IController), container);
            Assert.That(registeredControllers, Is.EqualTo(allControllers));
        }

        [Test]
        public void AllAndOnlyControllersHaveControllerSuffix()
        {
            System.Type[] allControllers = InstallerTestHelper.GetPublicClassesFromApplicationAssembly(c => c.Name.EndsWith("Controller"));
            System.Type[] registeredControllers = InstallerTestHelper.GetImplementationTypesFor(typeof(IController), container);
            System.Type[] registeredApiControllers = InstallerTestHelper.GetImplementationTypesFor(typeof(IHttpController), container);
            System.Type[] actual = registeredControllers.Concat(registeredApiControllers)
                                              .OrderBy(x => x.Name)
                                              .ToArray();
            Assert.That(actual, Is.EqualTo(allControllers));
        }

        [Test]
        public void AllControllersAreTransient()
        {
            Castle.MicroKernel.IHandler[] nonTransientControllers = InstallerTestHelper.GetAssignableHandlers(typeof(IController), container)
                .Where(c => c.ComponentModel.LifestyleType != LifestyleType.Transient)
                .ToArray();
            Assert.That(nonTransientControllers, Is.Empty);
        }

        [Test]
        public void AllControllersExposeThemselvesAsService()
        {
            Castle.MicroKernel.IHandler[] controllersWithWrongName = InstallerTestHelper.GetAssignableHandlers(typeof(IController), container)
                .Where(c => !c.ComponentModel.Services.SequenceEqual(new[] { c.ComponentModel.Implementation }))
                .ToArray();
            Assert.That(controllersWithWrongName, Is.Empty);
        }
    }
}