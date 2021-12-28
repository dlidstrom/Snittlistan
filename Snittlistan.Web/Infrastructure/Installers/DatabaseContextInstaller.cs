#nullable enable

using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Snittlistan.Web.Infrastructure.Database;

namespace Snittlistan.Web.Infrastructure.Installers;

public class DatabaseContextInstaller : IWindsorInstaller
{
    private readonly Func<Databases> databases;

    private readonly Func<ComponentRegistration<Databases>, ComponentRegistration<Databases>> func;

    public DatabaseContextInstaller(Func<Databases> databases)
    {
        func = x => x.LifestylePerWebRequest();
        this.databases = databases;
    }

    public DatabaseContextInstaller(Func<Databases> databases, LifestyleType lifestyleType)
    {
        func = x => x.LifeStyle.Is(lifestyleType);
        this.databases = databases;
    }

    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
        _ = container.Register(
            func.Invoke(
                Component.For<Databases>()
                    .UsingFactoryMethod(databases)));
    }
}
