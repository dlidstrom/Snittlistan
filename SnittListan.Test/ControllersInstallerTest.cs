using System;
using System.Linq;
using System.Web.Mvc;
using Castle.Core.Internal;
using Castle.MicroKernel;
using Castle.Windsor;
using SnittListan.Controllers;
using SnittListan.Installers;
using Xunit;
using Castle.Core;

namespace SnittListan.Test
{
	public class ControllersInstallerTest
	{
		private readonly IWindsorContainer container;

		public ControllersInstallerTest()
		{
			container = new WindsorContainer()
				.Install(new ControllerInstaller());
		}

		[Fact]
		public void AllControllersImplementIController()
		{
			var allHandlers = GetAllHandlers(container);
			var controllerHandlers = GetHandlersFor(typeof(IController), container);
			Assert.NotEmpty(allHandlers);
			Assert.Equal(allHandlers, controllerHandlers);
		}

		[Fact]
		public void AllControllersAreRegistered()
		{
			// Is<TType> is a helper extension method from Windsor
			// which behaves like 'is' keyword in C# but at a Type, not instance level

			var allControllers = GetPublicClassesFromApplicationAssembly(c => c.Is<IController>());
			var registeredControllers = GetImplementationTypesFor(typeof(IController), container);
			Assert.Equal(allControllers, registeredControllers);
		}

		[Fact]
		public void AllAndOnlyControllersHaveControllerSuffix()
		{
			var allControllers = GetPublicClassesFromApplicationAssembly(c => c.Name.EndsWith("Controller"));
			var registeredControllers = GetImplementationTypesFor(typeof(IController), container);
			Assert.Equal(allControllers, registeredControllers);
		}

		[Fact]
		public void AllAndOnlyControllersLiveInControllersNamespace()
		{
			var allControllers = GetPublicClassesFromApplicationAssembly(c => c.Namespace.Contains("Controllers"));
			var registeredControllers = GetImplementationTypesFor(typeof(IController), container);
			Assert.Equal(allControllers, registeredControllers);
		}

		[Fact]
		public void AllControllersAreTransient()
		{
			var nonTransientControllers = GetHandlersFor(typeof(IController), container)
				.Where(c => c.ComponentModel.LifestyleType != LifestyleType.Transient)
				.ToArray();
			Assert.Empty(nonTransientControllers);
		}

		[Fact]
		public void AllControllersExposeThemselvesAsService()
		{
			var controllersWithWrongName = GetHandlersFor(typeof(IController), container)
				.Where(c => c.Service != c.ComponentModel.Implementation)
				.ToArray();
			Assert.Empty(controllersWithWrongName);
		}

		private static IHandler[] GetAllHandlers(IWindsorContainer container)
		{
			return GetHandlersFor(typeof(object), container);
		}

		private static IHandler[] GetHandlersFor(Type type, IWindsorContainer container)
		{
			return container.Kernel.GetAssignableHandlers(type);
		}

		private static Type[] GetImplementationTypesFor(Type type, IWindsorContainer container)
		{
			return GetHandlersFor(type, container)
				.Select(h => h.ComponentModel.Implementation)
				.OrderBy(t => t.Name)
				.ToArray();
		}

		private static Type[] GetPublicClassesFromApplicationAssembly(Predicate<Type> where)
		{
			return typeof(HomeController).Assembly.GetExportedTypes()
				.Where(t => t.IsClass)
				.Where(t => t.IsAbstract == false)
				.Where(where.Invoke)
				.OrderBy(t => t.Name)
				.ToArray();
		}
	}
}
