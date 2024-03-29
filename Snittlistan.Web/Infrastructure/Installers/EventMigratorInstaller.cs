﻿
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Snittlistan.Web.Areas.V2.Migration;

#nullable enable

namespace Snittlistan.Web.Infrastructure.Installers;
public class EventMigratorInstaller : IWindsorInstaller
{
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
        _ = container.Register(
            Classes
                .FromThisAssembly()
                .BasedOn<IEventMigratorWithResults>()
                .WithServiceFromInterface(typeof(IEventMigratorWithResults))
                .LifestyleTransient());
    }
}
