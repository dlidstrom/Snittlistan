﻿using Castle.MicroKernel;
using Castle.Windsor;
using Snittlistan.Web.Areas.V1.Controllers;

namespace Snittlistan.Test;
public static class InstallerTestHelper
{
    public static IHandler[] GetAllHandlers(IWindsorContainer container)
    {
        return container.Kernel.GetAssignableHandlers(typeof(object));
    }

    public static IHandler[] GetHandlersFor(Type type, IWindsorContainer container)
    {
        return container.Kernel.GetHandlers(type);
    }

    public static IHandler[] GetAssignableHandlers(Type type, IWindsorContainer container)
    {
        return container.Kernel.GetAssignableHandlers(type);
    }

    public static Type[] GetImplementationTypesFor(Type type, IWindsorContainer container)
    {
        return GetAssignableHandlers(type, container)
            .Select(h => h.ComponentModel.Implementation)
            .OrderBy(t => t.Name)
            .ToArray();
    }

    public static Type[] GetPublicClassesFromApplicationAssembly(Predicate<Type> where)
    {
        return typeof(HomeController).Assembly.GetExportedTypes()
            .Where(t => t.IsClass)
            .Where(t => t.IsAbstract == false)
            .Where(where.Invoke)
            .OrderBy(t => t.Name)
            .ToArray();
    }
}
